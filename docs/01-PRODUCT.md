# The Chronicles - Product Specification

## 1. Product overview

The Chronicles is a private digital archive for shared memories.

Users create separate chronicles for different groups of people, such as family, friends, trips or other communities. Members of a chronicle collaboratively add, edit and explore memories on a shared interactive timeline.

The central emotional goal of the product is nostalgia: helping people preserve moments from the past and return to them in the future.

The product should feel personal, warm and meaningful rather than like a conventional social media platform.

## 2. Product principles

- Memories are the most important part of the product.
- Creating a memory should be quick and effortless.
- Memories without photos are just as valid as memories with photos.
- Chronicles are collaborative spaces.
- The timeline should encourage exploration and rediscovery.
- The interface should feel like a personal archive or scrapbook.
- The product should remain simple and focused in its initial release.

## 3. Core product structure

The application consists of:

1. User accounts.
2. Dashboard containing the user's chronicles.
3. Individual chronicles.
4. Shared timelines.
5. Memories and their details.
6. Chronicle members and invitations.
7. User profile and account settings.

A user can belong to multiple chronicles. Each chronicle has its own members, memories and timeline.

## 4. User accounts

### MVP functionality

- Account deletion with explicit confirmation and password verification.
- Registration using email and password.
- Login using email and password.
- Authentication based on JWT.
- Logout.
- Password recovery through an email link.
- Password reset using a time-limited, single-use link.
- Display name.
- Email address.
- Ability to change display name.
- Ability to change password.

Google authentication and other external authentication providers are outside the MVP scope.

Token storage and session security must be addressed during technical architecture design. Avoid insecure client-side token storage.

### Email verification

- Email verification is not required in the MVP.
- Users can access the application immediately after registration.
- Email verification may be introduced in a future version.

## 5. Chronicles

A chronicle is a private, shared space for preserving memories.

- Photos are private and accessible only to current members
  of the chronicle they belong to.

### Creating a chronicle

A user can create a chronicle by providing:

- Name — required.
- Description — optional.

After creation, the user can invite other people immediately or do it later.

### Dashboard

After logging in, users see a dashboard containing all chronicles they belong to.

Each chronicle is represented by a card containing:

- Chronicle name.
- Description, if available.
- Number of members.

Selecting a card opens that chronicle's timeline.

The dashboard also contains an action to create a new chronicle.

### Empty dashboard

If the user does not belong to any chronicles, display:

- A friendly illustration.
- A short welcoming message.
- A prominent "Create your first chronicle" action.

## 6. Chronicle membership and permissions

Chronicles are collaborative.

Every member can:

- View the chronicle and its timeline.
- Add memories.
- Edit memories.
- Delete memories.
- Invite new members using an invitation link.
- Leave the chronicle.

Only the current chronicle Owner can delete the entire chronicle.

Ownership belongs to the current Owner membership, not permanently to the
user who originally created the chronicle.

If the Owner leaves a chronicle with other members, ownership is transferred
to the oldest remaining member.

### Chronicle member limits

- There is no fixed maximum number of members per chronicle in the MVP.
- Any current member can invite additional people.
- The architecture should not assume a small, fixed number of members.
- Performance limitations may be evaluated in the future based on actual usage.

### Invitations

MVP invitation method:

- Generate a unique invitation link.
- Allow members to copy and share the link.
- Users opening the link can join the chronicle after authentication or registration.
- Invitation links can be revoked and regenerated.

Email invitations and user search are deferred until after the MVP.

### Invitation permissions

- Every current chronicle member can copy and share the active invitation link.
- Every current chronicle member can generate a replacement invitation link.
- Generating a replacement link immediately invalidates the previous link.
- Only the current Owner can delete the entire chronicle.

### Invitation link validity

- Invitation links do not expire automatically.
- Links remain active until explicitly revoked or replaced.
- Invitation links can be used by multiple people.
- Generating a replacement link automatically invalidates
  the previous link.

## 7. Memories

A memory represents an event or moment belonging to a chronicle.

### Memory fields

| Field        | Required | Description               |
| ------------ | -------- | ------------------------- |
| Title        | Yes      | Short name of the memory  |
| Date         | Yes      | Date of the event         |
| Photos       | No       | Multiple photos supported |
| Description  | No       | Short description         |
| Participants | No       | Members of the chronicle  |
| Location     | No       | Optional event location   |

### Date rules

- The default date is today.
- Users can select any date in the past.
- Future dates are not allowed.

### Photos

MVP supports multiple photos per memory.

Users can:

- Add multiple photos.
- Preview photos before saving.
- Remove individual photos.
- Change photo order.
- Add and remove photos while editing an existing memory.

The first photo in the selected order is the primary photo displayed on the timeline.

A memory without photos must remain visually complete and meaningful.

Photo limits in the MVP:

- Maximum 20 photos per memory.
- Maximum 10 MB per photo.
- Supported formats: JPEG, PNG and WebP.

### Participants

Users can select participants from the members of the chronicle.

### Memory editing

Every chronicle member can edit memories.

Editing includes all memory fields, including photos, participants and location.

### Memory deletion

Every chronicle member can delete memories.

Deletion requires explicit confirmation.

## 8. Timeline

Every chronicle has its own interactive timeline.

The timeline is the primary way of exploring memories.

### Zoom levels

The timeline has three visual information levels:

1. Overview — broad date range, many small event nodes, year and month labels.
2. Exploration — narrower date range, larger nodes, event titles and previews or thumbnails.
3. Memories — detailed view with larger memory cards displayed alternately above and below the central timeline.

Zooming is continuous and fluid, with snapping to the three defined levels.

### Navigation

- Mouse wheel controls zoom.
- Dragging moves the timeline horizontally.
- Plus and minus controls zoom in and out.
- A "Fit timeline" action returns to the full available timeline range.

### Memory appearance

Memories with photos use larger visual cards featuring their primary photo, title and description.

Memories without photos use more compact text-based cards.

Both variants must feel intentional and valuable.

### Selecting a memory

Selecting a node centers it on the screen by smoothly moving the entire timeline.

The selected memory opens in a details panel on the left.

The current zoom level does not change.

Selecting another node while the panel is open updates its content and moves the timeline to the newly selected memory.

## 9. User interface and platform scope

The MVP is designed for desktop and laptop screens.

- Minimum supported viewport width: 1280 px.
- Mobile and tablet layouts are outside the initial MVP scope.
- Architecture should not unnecessarily prevent responsive support in the future.

The application uses a persistent top navigation bar.

The timeline occupies the available space below the navigation bar rather than the entire viewport height.

Users can return to the dashboard and switch between chronicles without relying on browser navigation.

## 10. Visual identity

The product's visual direction is based on:

- Warm, aged beige and parchment backgrounds.
- Pencil and graphite-style illustrations.
- Sepia and dark brown tones.
- Editorial serif typography.
- Scrapbook and personal travel journal references.
- A nostalgic, intimate and timeless atmosphere.

The design should avoid looking like a generic social network or administrative dashboard.

## 11. MVP scope

### Included

- User registration and login.
- JWT-based authentication.
- Password recovery and reset.
- User profile basics.
- Dashboard with chronicle cards.
- Creating chronicles.
- Shared chronicle membership.
- Invitation links.
- Leaving and deleting chronicles according to permissions.
- Interactive timeline with three zoom levels.
- Adding memories.
- Multiple photos per memory.
- Photo ordering and primary photo selection.
- Memory details panel.
- Editing and deleting memories.
- Participants and optional location.
- Empty dashboard and empty timeline states.
- Real-time synchronization between chronicle members.
- Preservation of unsaved changes during connection loss.

- Memory reactions:
  - Love ❤️
  - Funny 😂
  - Moved 🥹
  - Surprised 😮
  - Each user can have one reaction per memory.
  - Selecting another reaction replaces the previous one.
  - Selecting the active reaction again removes it.
  - Reactions and their counts are visible to all chronicle members.

### Deferred

- Google authentication.
- Email-based invitations.
- Searching for users to invite.
- Mobile and tablet layouts.
- Public profiles and public chronicles.
- Likes, comments.
- Notifications.
- AI-generated images.
- Wrapped, statistics and memory summaries.
- Payments and subscriptions.
- Advanced social features.

## 12. Product development principles

- Implement features incrementally.
- Follow approved product and UX specifications.
- Do not introduce unapproved features.
- Discuss major technical and architectural decisions before implementation.
- Keep documentation aligned with approved decisions.
- Prefer simple solutions that satisfy the MVP requirements.
