# 06 - Data Model

## 1. Overview

The Chronicles uses PostgreSQL as its primary relational database.

Entity Framework Core is used for database access and migrations.

The database model is designed around chronicles, their members, memories and collaborative interactions.

The application uses UUIDv7 identifiers for its main entities.

## 2. Technology and Data Conventions

| Element                    | Convention                    |
| -------------------------- | ----------------------------- |
| Database                   | PostgreSQL 18                 |
| ORM                        | Entity Framework Core         |
| Primary identifiers        | UUIDv7                        |
| C# identifier type         | Guid                          |
| PostgreSQL identifier type | UUID                          |
| Date-only values           | PostgreSQL DATE / C# DateOnly |
| Timestamp values           | PostgreSQL TIMESTAMPTZ        |
| Authentication users       | ASP.NET Core Identity         |
| Photo binaries             | Separate file storage         |

### 2.1 UUIDv7

UUIDv7 is the standard identifier format.

Identifiers should be generated using PostgreSQL's native `uuidv7()` function.

This applies to application entities and user identifiers managed by ASP.NET Core Identity.

Entity Framework Core must be configured so that database-generated identifiers are not unnecessarily overwritten by application-side generators.

### 2.2 Dates and timestamps

Memory dates represent calendar days and use:

- PostgreSQL `DATE`
- C# `DateOnly`

Creation and modification timestamps use timestamp values.

Timestamp storage and application handling should consistently use UTC.

## 3. Entity Overview

The initial model consists of nine main entities:

1. User
2. Chronicle
3. ChronicleMembership
4. ChronicleInvitation
5. Memory
6. MemoryParticipant
7. MemoryPhoto
8. MemoryReaction
9. RefreshToken

## 4. Entity Definitions

### 4.1 User

Users are managed through ASP.NET Core Identity.

The application extends the Identity user model with application-specific profile information.

Proposed application field:

| Field       | Type             | Required | Description         |
| ----------- | ---------------- | -------- | ------------------- |
| Id          | UUID             | Yes      | User identifier     |
| DisplayName | VARCHAR          | Yes      | Public display name |
| Email       | Identity-managed | Yes      | User email          |

Other authentication-related fields are managed by ASP.NET Core Identity.

Profile photos are not included in MVP.

### 4.2 Chronicle

Represents an individual shared chronicle.

| Field           | Type        | Required | Description                                      |
| --------------- | ----------- | -------- | ------------------------------------------------ |
| Id              | UUID        | Yes      | Chronicle identifier                             |
| Name            | VARCHAR     | Yes      | Chronicle name                                   |
| Description     | TEXT        | No       | Optional description                             |
| CreatedAt       | TIMESTAMPTZ | Yes      | Creation timestamp                               |
| CreatedByUserId | UUID        | No       | Original creator; NULL if the account is deleted |

Relationships:

- One chronicle has many memberships.
- One chronicle has many memories.
- One chronicle has invitation records.
- Every chronicle must have exactly one current Owner.

The original creator is recorded separately from current ownership.

### 4.3 ChronicleMembership

Explicit junction entity connecting users and chronicles.

This is not implemented as an implicit many-to-many relationship.

| Field       | Type        | Required | Description                   |
| ----------- | ----------- | -------- | ----------------------------- |
| Id          | UUID        | Yes      | Membership identifier         |
| UserId      | UUID        | Yes      | Member user                   |
| ChronicleId | UUID        | Yes      | Chronicle                     |
| Role        | ENUM        | Yes      | Owner or Member               |
| JoinedAt    | TIMESTAMPTZ | Yes      | Membership creation timestamp |

Allowed roles:

- Owner
- Member

Rules:

1. Every chronicle has exactly one Owner.
2. Every chronicle creator automatically receives an Owner membership.
3. Other members have the Member role.
4. A user can have only one membership in a given chronicle.
5. The pair `UserId + ChronicleId` must be unique.
6. Membership creation and ownership assignment must be performed consistently.

Ownership transfer:

- If an Owner leaves a chronicle containing other members, ownership passes to the oldest remaining member.
- The new Owner receives the Owner role.
- The previous Owner's membership is deleted.
- Ownership transfer requires confirmation in the interface.
- If multiple members have the same `JoinedAt`, a deterministic tie-breaker must be defined during implementation.

If the Owner is the only member:

- Leaving the chronicle is disabled.
- Deleting the chronicle remains available.
- Deleting the chronicle permanently removes its data.

### 4.4 ChronicleInvitation

Stores invitation link information.

| Field           | Type        | Required | Description                                                                      |
| --------------- | ----------- | -------- | -------------------------------------------------------------------------------- |
| Id              | UUID        | Yes      | Invitation identifier                                                            |
| ChronicleId     | UUID        | Yes      | Associated chronicle                                                             |
| TokenHash       | TEXT        | Yes      | Cryptographic hash used to verify the invitation token                           |
| EncryptedToken  | TEXT        | Yes      | Encrypted original invitation token, used to retrieve the active invitation link |
| CreatedByUserId | UUID        | No       | User who generated the link                                                      |
| CreatedAt       | TIMESTAMPTZ | Yes      | Generation timestamp                                                             |
| RevokedAt       | TIMESTAMPTZ | No       | Revocation timestamp                                                             |

**Rules:**

1. A chronicle can have at most one active invitation link.
2. Invitation links do not expire automatically.
3. Generating a new link revokes the previous active link.
4. A revoked token cannot be used to join.
5. The original token must never be stored in plain text.
6. The original token is encrypted before being stored.
7. TokenHash is used for invitation verification.
8. EncryptedToken allows authorized members to retrieve and copy the active invitation link.
9. Only current chronicle members can retrieve the active invitation link.
10. Encryption keys must be stored securely outside the database.
11. Invitation tokens must never be exposed in application logs.

A partial unique index should enforce at most one active invitation per chronicle, where `RevokedAt IS NULL`.

The original invitation token is returned to the authorized member when the invitation is generated. It can subsequently be retrieved by decrypting EncryptedToken.

Generating a replacement token invalidates the previous token.

### 4.5 Memory

Represents an individual memory belonging to exactly one chronicle.

| Field           | Type        | Required | Description                 |
| --------------- | ----------- | -------- | --------------------------- |
| Id              | UUID        | Yes      | Memory identifier           |
| ChronicleId     | UUID        | Yes      | Associated chronicle        |
| CreatedByUserId | UUID        | No       | Original author             |
| UpdatedByUserId | UUID        | No       | Last editor                 |
| Title           | VARCHAR     | Yes      | Memory title                |
| Date            | DATE        | Yes      | Actual memory date          |
| Description     | TEXT        | No       | Optional description        |
| Location        | TEXT        | No       | Optional textual location   |
| CreatedAt       | TIMESTAMPTZ | Yes      | Creation timestamp          |
| UpdatedAt       | TIMESTAMPTZ | Yes      | Last modification timestamp |

Rules:

1. Every memory belongs to exactly one chronicle.
2. A memory cannot be shared between different chronicles.
3. The title is required.
4. The memory date is required.
5. Future memory dates are prohibited.
6. Description and location are optional.
7. Memories without photos remain fully valid.
8. Every chronicle member can edit and delete memories.

Authorship:

- `CreatedByUserId` records the original author.
- `UpdatedByUserId` records the last editor.
- The author does not have exclusive editing rights.

When a user account is deleted:

- Existing memories remain.
- References to the deleted user become NULL.
- The interface displays "Deleted user".

No full memory revision history or activity log is included in MVP.

### 4.6 MemoryParticipant

Explicit junction entity connecting memories and their participants.

| Field    | Type | Required | Description       |
| -------- | ---- | -------- | ----------------- |
| MemoryId | UUID | Yes      | Associated memory |
| UserId   | UUID | Yes      | Participant       |

Rules:

1. A memory can have multiple participants.
2. A user can participate in multiple memories.
3. A user can only be assigned as a participant if they belong to the chronicle containing the memory.
4. The pair `MemoryId + UserId` must be unique.
5. Participants are selected from existing chronicle members.

The database must enforce that a participant belongs to the same chronicle as the memory.

Leaving a chronicle:

- Removes the user's membership.
- Removes their participant assignments in that chronicle.
- Does not remove memories.
- Does not change memory descriptions, dates or photos.

Rejoining:

- Creates a new membership.
- Does not automatically restore previous participant assignments.
- The user can be manually assigned again to existing memories.

### 4.7 MemoryPhoto

Stores metadata and references for photos associated with memories.

| Field       | Type    | Required | Description                 |
| ----------- | ------- | -------- | --------------------------- |
| Id          | UUID    | Yes      | Photo identifier            |
| MemoryId    | UUID    | Yes      | Associated memory           |
| StorageKey  | TEXT    | Yes      | Storage location identifier |
| FileName    | TEXT    | Yes      | Original filename           |
| ContentType | VARCHAR | Yes      | File MIME type              |
| FileSize    | BIGINT  | Yes      | File size in bytes          |
| SortOrder   | INTEGER | Yes      | Display order               |

Rules:

1. A memory can contain multiple photos.
2. Photos are optional.
3. Maximum 20 photos per memory.
4. Maximum 10 MB per photo.
5. Supported formats: JPEG, PNG and WebP.
6. Every photo has its own UUIDv7 identifier.
7. Physical storage names are generated independently of original filenames.
8. `FileName` is metadata only.
9. `StorageKey` identifies the actual stored file.
10. The first photo has `SortOrder = 0` and is the primary photo.
11. SortOrder values are continuous: 0, 1, 2, 3...
12. Changing photo order must be performed atomically.

Photo deletion:

- Removes the database metadata.
- Removes the physical file from storage.
- Reorders remaining photos.
- If the primary photo is removed, the next photo becomes primary.
- If all photos are removed, the memory remains valid.

The application must validate uploaded file contents, not rely exclusively on the filename or supplied MIME type.

### 4.8 MemoryReaction

Stores user reactions to memories.

| Field        | Type        | Required | Description         |
| ------------ | ----------- | -------- | ------------------- |
| Id           | UUID        | Yes      | Reaction identifier |
| MemoryId     | UUID        | Yes      | Associated memory   |
| UserId       | UUID        | Yes      | Reacting user       |
| ReactionType | ENUM        | Yes      | Reaction type       |
| CreatedAt    | TIMESTAMPTZ | Yes      | Creation timestamp  |

Allowed reaction types:

- Love
- Funny
- Moved
- Surprised

Corresponding emoji:

- ❤️
- 😂
- 🥹
- 😮

Rules:

1. Every user can have at most one reaction per memory.
2. The pair `MemoryId + UserId` must be unique.
3. Selecting a different reaction replaces the previous reaction.
4. Clicking the current reaction again removes it.
5. Reactions are visible to all chronicle members.
6. Members can see reaction counts and who reacted.
7. Only members of the associated chronicle can react.
8. Removing membership removes the user's reactions in that chronicle.

### 4.9 RefreshToken

Stores refresh token records associated with individual user sessions.

A user can have multiple independent sessions across different devices and browsers.

Each refresh token record represents one generation of a refresh token. Multiple records can belong to the same session through a stable `SessionId`.

| Field             | Type        | Required | Description                                                          |
| ----------------- | ----------- | -------- | -------------------------------------------------------------------- |
| Id                | UUID        | Yes      | Refresh token record identifier                                      |
| SessionId         | UUID        | Yes      | Stable identifier shared by all token generations within one session |
| UserId            | UUID        | Yes      | Session owner                                                        |
| TokenHash         | TEXT        | Yes      | Cryptographic hash of the refresh token                              |
| CreatedAt         | TIMESTAMPTZ | Yes      | Token creation timestamp                                             |
| ExpiresAt         | TIMESTAMPTZ | Yes      | Token expiration timestamp                                           |
| RevokedAt         | TIMESTAMPTZ | No       | Token revocation timestamp                                           |
| ReplacedByTokenId | UUID        | No       | ID of the token that replaced this token                             |
| DeviceType        | VARCHAR     | No       | Desktop, Mobile, Tablet or Unknown                                   |
| Browser           | VARCHAR     | No       | Browser name, e.g. Chrome, Firefox or Safari                         |
| OperatingSystem   | VARCHAR     | No       | Operating system, e.g. Windows, macOS or Android                     |
| LastActivityAt    | TIMESTAMPTZ | No       | Last recorded session activity                                       |

**Rules:**

1. Every refresh token belongs to exactly one user.
2. A user can have multiple independent sessions.
3. Every session has its own stable `SessionId`.
4. Every refresh token generation has its own `Id`.
5. Refresh tokens are stored only as cryptographic hashes, never in plain text.
6. Refresh tokens are rotated after every successful refresh operation.
7. The previous token is revoked when a replacement token is issued.
8. `ReplacedByTokenId` references the token that replaced the current token, when applicable.
9. A session can have only one active refresh token at a time.
10. Refresh token expiration follows the authentication policy defined in the technical architecture.
11. Revoked token records must remain available for detecting reuse of previously revoked tokens.
12. Reuse of a revoked refresh token triggers revocation of all active sessions belonging to the affected user.
13. Device and browser information is informational metadata and must not be treated as a reliable device identity.
14. IP addresses are not stored as part of refresh token or session records.
15. Device metadata should be updated or carried forward when a token is rotated.
16. `LastActivityAt` records the most recent relevant session activity.

**Session management:**

- Users can have multiple active sessions.
- Users should be able to view their active sessions.
- Users should be able to revoke an individual session.
- Users should be able to revoke all other active sessions.
- Revoking a session invalidates its active refresh token.
- A session's identity remains stable across refresh token rotations.

The session management interface is planned for future implementation and is not required for MVP.

## 5. Relationships

### 5.1 User and Chronicle

Many-to-many through `ChronicleMembership`.

- One user can belong to multiple chronicles.
- One chronicle can have multiple members.
- Every membership has exactly one role.

### 5.2 Chronicle and Memory

One-to-many.

- One chronicle contains multiple memories.
- Each memory belongs to exactly one chronicle.

### 5.3 Memory and MemoryParticipant

One-to-many from Memory to MemoryParticipant.

- One memory can have multiple participants.
- Each participant record references one user.

### 5.4 Memory and MemoryPhoto

One-to-many.

- One memory can have multiple photos.
- Each photo belongs to exactly one memory.

### 5.5 Memory and MemoryReaction

One-to-many.

- One memory can have multiple reactions from different users.
- Each user can have only one reaction per memory.

### 5.6 Chronicle and ChronicleInvitation

One-to-many historically.

- Invitation records preserve the history of generated and revoked links.
- At most one invitation can be active at a time.

### 5.7 User and RefreshToken

One-to-many.

One user can have multiple refresh token records.

Every refresh token record belongs to exactly one user.

Multiple token generations can belong to the same session through SessionId.

A session can have only one active refresh token at a time.

Revoking a session invalidates its active refresh token.

## 6. Referential Integrity and Constraints

The database must enforce important domain invariants wherever practical.

### Unique constraints and indexes

| Entity              | Constraint                                  |
| ------------------- | ------------------------------------------- |
| ChronicleMembership | UNIQUE(UserId, ChronicleId)                 |
| MemoryParticipant   | UNIQUE(MemoryId, UserId)                    |
| MemoryReaction      | UNIQUE(MemoryId, UserId)                    |
| ChronicleInvitation | At most one active invitation per Chronicle |
| MemoryPhoto         | UNIQUE(MemoryId, SortOrder)                 |
| RefreshToken        | UNIQUE(TokenHash)                           |
| RefreshToken        | Index(UserId, SessionId)                    |
| RefreshToken        | Index(SessionId, RevokedAt)                 |
| RefreshToken        | Index(ExpiresAt)                            |

The ReplacedByTokenId field references another record in the RefreshToken table.

A session must not have multiple active refresh tokens.

This invariant must be enforced through an appropriate database constraint or transactional backend logic.

Additional useful indexes:

- ChronicleMembership by UserId.
- ChronicleMembership by ChronicleId.
- Memory by ChronicleId and Date.
- MemoryPhoto by MemoryId and SortOrder.
- MemoryReaction by MemoryId.
- ChronicleInvitation by ChronicleId.

### Ownership constraints

Every chronicle must have exactly one current Owner.

Ownership transfer must:

- Promote the selected successor.
- Remove or demote the previous Owner.
- Preserve the single-Owner invariant.

These operations must be atomic.

### Participant integrity

The database must prevent assigning a user to a memory if that user is not a member of the memory's chronicle.

The exact implementation may use composite foreign keys with additional ChronicleId references or another database-level mechanism.

### Reaction integrity

Only current chronicle members may react to memories.

The backend must enforce this rule. Database-level enforcement should be considered alongside participant integrity.

## 7. Deletion Rules

### 7.1 Deleting a Chronicle

Deleting a chronicle permanently deletes all associated data:

- Chronicle memberships.
- Memories.
- Memory participants.
- Memory photos and metadata.
- Memory reactions.
- Invitation records.
- Physical photo files from storage.

The database deletion should be transactional.

Physical file deletion is handled asynchronously using the Outbox Pattern
and a Background Worker.

File deletion tasks must be recorded in the same PostgreSQL transaction
as the corresponding database deletion.

The Background Worker processes pending tasks and deletes physical files
from storage.

Failed tasks must remain available for retry.

File deletion operations must be idempotent.

### 7.2 Deleting a Memory

Deleting a memory permanently deletes:

- Memory record.
- Participant assignments.
- Photo metadata.
- Reactions.
- Physical photo files.

Deletion requires explicit user confirmation.

Memories cannot be restored after deletion.

### 7.3 Removing a Chronicle Member

Removing or leaving a chronicle:

- Deletes the membership record.
- Deletes participant assignments in that chronicle.
- Deletes reactions in that chronicle.
- Preserves all memories and photos.

If the removed user is the Owner, ownership must first be transferred according to the ownership rules.

### 7.4 Deleting a User Account

Deleting an account requires:

- An explicit confirmation.
- Re-entering the user's password.
- Backend verification before deletion.

Refresh token and session handling:

All refresh token records associated with the deleted user must be invalidated.

No active session may remain usable after account deletion.

Refresh token records must not be retained longer than necessary for security and operational requirements.

For every chronicle owned by the user:

**If other members exist:**

- Ownership transfers to the oldest remaining member.
- The previous Owner is removed.

**If the user is the only member:**

- The chronicle is permanently deleted with all associated data.

For chronicles where the user is a regular Member:

- Membership is deleted.
- Participant assignments are deleted.
- Reactions are deleted.
- Memories remain.

Memory authorship:

- `CreatedByUserId` becomes NULL if the referenced user is deleted.
- `UpdatedByUserId` becomes NULL if the referenced user is deleted.
- The interface displays "Deleted user".

Other users' data and memories must remain intact.

The account deletion workflow must handle multiple chronicles and external file cleanup safely.

## 8. Photo Storage

Photo binaries are not stored directly in PostgreSQL.

The database stores:

- Photo identifiers.
- Original filenames.
- Content types.
- File sizes.
- Storage keys.
- Ordering information.

Initial development uses local file storage.

The long-term direction is Cloud Object Storage.

The backend should use a storage abstraction so that the storage provider can be replaced without rewriting memory business logic.

## 9. Security and Data Integrity

Important security rules:

Every protected operation requires backend authorization.

Membership must be checked for chronicle-specific operations.

Users cannot access memories outside their chronicles.

Participants must be members of the same chronicle.

Reactions are limited to chronicle members.

Invitation tokens are stored as cryptographic hashes for verification and in encrypted form for authorized link retrieval.

Encryption keys used for invitation tokens must be stored separately from the database.

Refresh tokens are stored only as cryptographic hashes.

Refresh token rotation and revocation must be enforced by the backend.

Reuse of a revoked refresh token invalidates all active sessions belonging to the user.

Uploaded photos must be validated.

Database constraints must protect critical uniqueness and relationship rules.

Ownership changes and destructive operations must be atomic where possible.

## 10. Open Technical Decisions

The following details still require implementation-level design:

- Exact maximum lengths for text fields.
- Exact PostgreSQL enum versus string representation for roles and reaction types.
- Exact composite foreign key strategy for participant and reaction membership validation.
- Deterministic tie-breaking when members have identical JoinedAt timestamps.
- Exact Entity Framework Core configuration for UUIDv7 generation.
- Exact Outbox table structure and task status representation.
- Retry policy and handling of permanently failing file deletion tasks.
- Background Worker polling interval and batch size.
- Outbox task retention and cleanup policy.
- Exact transaction boundaries for account deletion and ownership transfers.
- Detailed database migration and seeding strategy.
- Exact refresh token cleanup and retention policy.
- Exact session activity update policy.
- Exact device and browser metadata parsing strategy.
- Exact database constraint and transaction strategy for refresh token rotation and session revocation.
- Exact implementation of concurrent memory updates and field-level conflict handling.
- Exact active editing presence detection and expiration mechanism.
- Exact real-time synchronization recovery protocol.

These details must be resolved before the relevant implementation.

## 11. Data Model Principles

1. Every memory belongs to exactly one chronicle.
2. Every chronicle has exactly one Owner.
3. Membership is represented by an explicit junction entity.
4. Participants must belong to the same chronicle as their memories.
5. Memories without photos are fully valid.
6. Removing members must not remove memories.
7. Deleting a chronicle permanently removes all its data.
8. Deleting an account must preserve other members' memories.
9. Photos are stored outside the relational database.
10. Critical domain rules must be enforced on the backend and, where practical, in PostgreSQL.

This document records the approved data model and its current technical direction. Unresolved implementation details are not automatically approved requirements.
