# The Chronicles - UX Flows and Interaction Specification

## 1. General interface structure

The MVP is desktop-first.

Minimum supported viewport width: 1280 px.

The application contains a persistent top navigation bar and a main content area below it.

The top navigation remains visible while users interact with and navigate the timeline.

The timeline must not occupy 100% of the viewport height.

Users should be able to return to the dashboard and switch chronicles without using browser back navigation.

## 2. Authentication flow

### Registration

1. User opens the registration page.
2. User provides an email address and password.
3. User submits the form.
4. Application validates the data.
5. On success, the user is authenticated and can access the application.

### Login

1. User opens the login page.
2. User enters email and password.
3. User submits the form.
4. Application validates credentials.
5. On success, the user enters the dashboard.

### Password recovery

1. User selects "Forgot password".
2. User provides their email address.
3. Application sends a password reset link.
4. User opens the link.
5. User sets a new password.
6. The link becomes invalid after use or expiration.

## 3. Dashboard

The dashboard displays all chronicles the user belongs to.

Each chronicle card contains:

- Name.
- Description, if available.
- Number of members.

Selecting a card opens its timeline.

The dashboard includes a persistent action for creating a new chronicle.

### Empty dashboard state

When the user has no chronicles:

- Show a subtle illustration in the product's visual style.
- Display a short welcoming message.
- Show a prominent "Create your first chronicle" button.

## 4. Creating a chronicle

1. User selects "Create chronicle".
2. A creation form appears.
3. User enters a required name.
4. User may enter an optional description.
5. User submits the form.
6. The chronicle is created.
7. User can invite other people immediately or skip this step.
8. User can open the new chronicle's timeline.

## 5. Inviting members

1. A member opens the invitation functionality.
2. Application generates or displays an invitation link.
3. Member copies the link.
4. They share it outside the application.
5. The recipient opens the link.
6. If necessary, the recipient registers or logs in.
7. The recipient joins the chronicle.

Every chronicle member can invite others.

Invitation links must be revocable and replaceable.

### Invitation link management

- Members can copy and share the active invitation link.
- The invitation link has no automatic expiration date.
- Members can generate a replacement link.
- Generating a replacement link immediately invalidates
  the previous link.
- Users attempting to join through an invalidated link
  are informed that the invitation is no longer valid.
- Every current member can copy the active invitation link.
- Every current member can generate a replacement link.
- Generating a replacement link immediately invalidates the previous link.

## 6. Timeline structure

The timeline is the central interaction area of a chronicle.

It remains below the persistent navigation bar.

The timeline has three zoom levels.

### Level 1 — Overview

- Displays a broad period of time, potentially spanning multiple years.
- Contains many small event nodes.
- Shows year and month labels.
- Does not display fully developed event cards.

### Level 2 — Exploration

- Displays a narrower period.
- Nodes become larger.
- Event titles and previews or thumbnails become visible.
- Users can identify individual memories more easily.

### Level 3 — Memories

- The timeline remains the central visual element.
- Larger memory cards appear alternately above and below the timeline.
- Cards provide more information about individual memories.

### Zoom behavior

Zooming is continuous and fluid, with snapping to the three defined levels.

Controls:

- Mouse wheel — zoom in and out.
- Plus button — zoom in.
- Minus button — zoom out.
- Fit timeline — return to the full available date range.

Dragging moves the timeline horizontally.

Exact zoom thresholds, animation durations and zoom anchoring behavior remain to be specified during interaction design and implementation planning.

## 7. Memory appearance

### Memories with photos

- Use larger cards.
- Display the primary photo.
- Display title and description.
- Visually enrich the timeline.

### Memories without photos

- Use more compact text-based cards.
- Display title and available text.
- Remain visually intentional and valuable.
- Must not look like incomplete or invalid memories.

Photo presence should influence visual richness but must not be the only factor determining a memory's importance.

## 8. Opening memory details

When a user selects a timeline node:

1. The entire timeline smoothly moves.
2. The selected node becomes centered on the screen.
3. A details panel slides in from the left.
4. The panel has a consistent fixed width on desktop.
5. The panel may partially cover the timeline.

The selected node should remain visible behind the panel.

The timeline is centered relative to the screen, not relative to the remaining uncovered area beside the panel.

### Details panel content

The panel always has the same structure, regardless of the current zoom level.

It displays:

- Photo gallery, if photos exist.
- Title.
- Date.
- Description, if available.
- Participants, if available.
- Location, if available.

The panel contains an edit action represented by a pencil icon.

It also contains an X button in the upper-right corner.

### Selecting another memory

When the details panel is already open and the user selects another node:

- The timeline moves to center the newly selected node.
- The current panel content is replaced with the new memory's details.
- The panel remains open.
- The zoom level remains unchanged.

### Closing the details panel

The panel closes only through its X button.

After closing:

- The timeline remains at its current position.
- The current zoom level is preserved.
- The user can continue exploring from the selected memory.

## 9. Editing a memory

Selecting the pencil icon switches the existing details panel into edit mode.

No separate modal or new window is opened.

The panel remains in the same position.

All memory fields become editable:

- Title.
- Date.
- Description.
- Photos.
- Photo order.
- Participants.
- Location.

The bottom of the panel displays:

- Save.
- Cancel.

### Saving

1. User changes memory data.
2. User selects Save.
3. Changes are saved.
4. The panel returns to details mode.
5. Timeline representation is updated.

### Canceling

Selecting Cancel discards unsaved changes and returns to details mode.

### Deleting a memory

The delete action is available inside the editing menu.

1. User selects Delete.
2. Application displays a confirmation dialog.
3. User confirms deletion.
4. Memory is deleted from the chronicle.

All chronicle members have permission to edit and delete memories.

## 10. Closing edit mode with unsaved changes

If the user selects X while there are unsaved changes, the application displays a confirmation dialog.

Available actions:

- Discard changes — discard modifications and close the panel.
- Return to editing — close the dialog and continue editing.

If there are no unsaved changes, X closes the panel immediately.

## 11. Adding a memory

A permanent "Add memory" button is available in the interface.

It is separate from the timeline zoom controls.

The plus and minus controls are reserved exclusively for zooming.

Selecting "Add memory" opens a modal centered on the screen.

### Modal appearance

- Timeline remains visible behind the modal.
- Background is slightly darkened.
- Background is lightly blurred.
- Modal is visually prominent.

Closing the modal returns the user to the exact previous timeline position and zoom level.

### Form fields

| Field        | Required |
| ------------ | -------- |
| Title        | Yes      |
| Date         | Yes      |
| Photos       | No       |
| Description  | No       |
| Participants | No       |
| Location     | No       |

### Date behavior

- Default date is today.
- Users can select dates in the past.
- Future dates are not allowed.

### Photo behavior

- Multiple photos can be added.
- Users can preview selected photos.
- Individual photos can be removed.
- Users can reorder photos.
- The first photo becomes the primary photo.
- The primary photo is used on the timeline.

### Form submission

1. User fills in the required title and date.
2. User optionally adds other information.
3. User selects Save.
4. Application creates the memory.
5. The new memory appears on the timeline.

The form must support creating a memory without photos or a description.

## 12. Empty timeline state

When a chronicle has no memories:

- Display the timeline without nodes.
- Show a subtle illustration in the product's visual style.
- Display a short message encouraging the user to add the first memory.
- Show a prominent "Add first memory" button.

The button opens the same add-memory modal used elsewhere.

## 13. User profile

The profile area provides:

- Display name.
- Email address.
- Change display name.
- Change password.
- Logout.

Profile photos are outside the MVP scope.

### Account deletion

Account deletion requires:

1. The user selects Delete account.
2. The application explains that account deletion is permanent.
3. The user must explicitly confirm the action.
4. The user re-enters their password.
5. The backend verifies the password before proceeding.
6. The account deletion process follows the rules defined in
   `06-DATA-MODEL.md`.

The user must be informed that:

- Chronicles with other members will be preserved and ownership transferred.
- Chronicles where the user is the only member will be permanently deleted.
- Memories in other users' chronicles will remain.
- The user's authorship references will be replaced with "Deleted user".

## 14. Chronicle membership

Every member can:

- View the chronicle and its timeline.
- Add memories.
- Edit memories.
- Delete memories.
- Invite other members.
- Leave the chronicle.

Every chronicle has exactly one current Owner.

Only the current Owner can delete the entire chronicle.

### Owner leaving a chronicle

If the Owner leaves a chronicle with other members:

1. The oldest remaining member becomes the new Owner.
2. The previous Owner's membership is removed.
3. The user must confirm the ownership transfer before leaving.

If the Owner is the only member:

- Leaving the chronicle is disabled.
- The Owner must delete the chronicle instead.
- Deleting the chronicle permanently removes all associated data.

## 15. Visual direction

The interface follows the approved visual identity:

- Warm aged beige and parchment backgrounds.
- Pencil and graphite-style illustrations.
- Sepia and dark brown tones.
- Editorial serif typography.
- Scrapbook and personal journal references.
- Nostalgic, intimate atmosphere.

The interface should feel like exploring a personal archive rather than browsing a generic social network.

## Memory reactions

Each memory supports four reaction types:

- Love ❤️
- Funny 😂
- Moved 🥹
- Surprised 😮

Each user can have only one reaction per memory.

Interaction rules:

- Selecting a reaction adds it to the memory.
- Selecting a different reaction replaces the current reaction.
- Selecting the active reaction again removes it.
- All chronicle members can see reactions and their counts.
- Users can see who reacted.
- Only current chronicle members can react.

## 16. Explicitly deferred UX

The following are not part of the initial MVP:

- Mobile and tablet layouts.
- Google authentication.
- Email-based invitations.
- User search and discovery.
- Comments.
- Notifications.
- Public profiles.
- Public chronicles.
- AI-generated images.
- Wrapped and statistics.

## 17. Open UX decisions

The following details have not yet been approved and should not be invented during implementation:

- Exact dimensions and responsive behavior of the details panel beyond its fixed desktop width.
- Exact zoom thresholds and animation durations.
- Zoom anchoring behavior.
- Exact layout of the photo gallery.
- Exact design of the add-memory form.
- Detailed interaction and validation states for forms.
- Mobile and tablet behavior.
