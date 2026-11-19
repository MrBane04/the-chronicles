# The Chronicles - Timeline Specification

## 1. Overview

The timeline is the central feature of The Chronicles.

It represents the chronological history of a chronicle and allows members to explore shared memories across time.

The timeline should feel like a living, continuously growing archive rather than a conventional data visualization.

Its visual identity is inspired by hand-drawn pencil lines, old journals and personal scrapbooks.

## 2. Core principles

- Time must be represented proportionally to its real duration.
- Every memory has its own identity and its own node.
- Empty periods remain visible.
- The timeline grows naturally as time passes.
- Users control navigation and zoom independently.
- Visual collision handling must not alter the actual chronological order.
- The timeline must remain usable even with a large number of memories.
- Interactions should feel smooth, natural and predictable.

## 3. Timeline direction and time scale

The timeline is horizontal.

- Past is on the left.
- Present is on the right.
- Users navigate horizontally.
- Memories are positioned according to their actual dates.

The timeline uses a real proportional time scale.

The distance between two dates should reflect the actual time elapsed between them.

For example, the distance between events separated by three years should be greater than the distance between events separated by one week.

Periods without memories remain visible.

The system may adjust the visual placement of individual nodes to prevent collisions, but the underlying chronological positions and dates must remain unchanged.

## 4. Timeline boundaries

### Beginning

The timeline begins at the date of the earliest memory in the chronicle.

A special marker identifies this point.

Suggested label:

"Your story begins here"

If a new memory is added with a date earlier than the current first memory, the beginning of the timeline moves accordingly.

### Present

The right boundary represents the current date.

A special dynamic marker displays:

"Today"

The marker updates automatically as time passes.

The timeline does not require a new memory to advance its present boundary.

### Navigation boundaries

Users cannot navigate before the earliest memory or beyond the current date.

The left boundary changes when an earlier memory is added.

The right boundary advances automatically with time.

## 5. Timeline zoom

The timeline has three visual information levels.

Zooming is continuous and fluid, with snapping to the three defined levels.

### Level 1 — Overview

Purpose: understanding the broad history of a chronicle.

Characteristics:

- Displays a wide date range.
- Shows many small event nodes.
- Displays year and month labels.
- Does not show fully developed memory cards.
- Uses compact node representations in dense periods.

### Level 2 — Exploration

Purpose: exploring a specific period.

Characteristics:

- Displays a narrower date range.
- Nodes become larger.
- Memory titles become visible.
- Previews and thumbnails may appear.
- Dense periods are displayed with more detail.

### Level 3 — Memories

Purpose: exploring individual memories in detail.

Characteristics:

- The timeline remains the central visual element.
- Larger memory cards appear alternately above and below the axis.
- Cards display more information.
- The layout automatically prevents cards from overlapping where possible.

### Zoom controls

- Mouse wheel controls zoom in and out.
- Plus button zooms in.
- Minus button zooms out.
- Fit timeline returns to the full available timeline range.

The exact zoom thresholds, animation durations and zoom anchoring behavior remain implementation decisions to be defined during technical planning.

## 6. Horizontal navigation

Users can drag the timeline horizontally.

Movement should feel fluid and may include subtle inertial scrolling.

The timeline must respect its boundaries.

The user can move between the earliest memory and the present without entering nonexistent time ranges.

## 7. Time navigation slider

A separate time navigation slider allows users to move quickly across long periods.

It is independent of the zoom controls.

### Behavior

- The slider represents the complete range from the earliest memory to today.
- Dragging the slider moves the main timeline immediately.
- The timeline responds continuously while the user drags.
- The current zoom level remains unchanged.
- A dynamic label above the slider displays the currently selected year.
- Users can quickly return to the present.

The slider is a navigation mechanism, not a zoom mechanism.

Its movement must not automatically change the timeline's zoom level.

## 8. Timeline visual appearance

The main axis resembles a line drawn by hand with a pencil.

Visual characteristics:

- Slightly irregular line.
- Graphite or dark brown color.
- Subtle imperfections.
- Natural, hand-drawn appearance.
- Matching date markers and event nodes.

The irregularity is purely visual.

Actual memory positions must continue to represent chronological time.

## 9. Memory nodes

Every memory has its own node.

Memories from the same date must not be merged into a single node.

### Memories with photos

- May use a photo thumbnail as the node representation.
- Their cards can be visually richer.
- The primary photo is the first photo in the memory's selected photo order.

### Memories without photos

- Use a hand-drawn graphical node.
- Remain visually meaningful and complete.
- Must not appear as incomplete or invalid memories.

Photos influence visual richness but must not be the only factor determining the importance of a memory.

### Node size

Node size changes smoothly according to the current zoom level.

## 10. Node interaction states

Every node has three primary visual states.

### Normal

The node uses its default hand-drawn appearance.

### Hover

When the pointer moves over a node:

- The node becomes slightly larger.
- A subtle visual highlight appears.
- The transition is short and smooth.

### Selected

The selected node receives a more visible highlight.

Suggested treatment:

- Double graphite outline.
- Subtle contrast increase.

The selected state remains visible while the corresponding details panel is open.

## 11. Memories with identical dates

Memories created for the same date remain separate nodes.

They must not be combined into a single event.

When multiple memories share a date:

- Nodes are positioned close to their actual chronological position.
- Nodes are distributed slightly above and below the axis.
- Nodes must not overlap.
- Small additional dots may indicate a higher concentration of memories.

Every node remains independently selectable.

## 12. Dense memory periods and collision handling

Some periods may contain many memories within a short period.

The timeline must automatically adjust the visual arrangement to preserve readability.

### Requirements

- Detect overlapping nodes and memory cards.
- Distribute elements above and below the axis.
- Prevent overlap wherever reasonably possible.
- Preserve chronological ordering.
- Preserve each memory's actual date.
- Keep each node independently selectable.

The visual position of a node may be adjusted slightly to avoid collisions.

This adjustment must never change the underlying date or chronological order.

### Density across zoom levels

| Level       | Behavior                                      |
| ----------- | --------------------------------------------- |
| Overview    | Compact nodes and minimal detail              |
| Exploration | Larger nodes, titles and thumbnails           |
| Memories    | Full memory cards distributed around the axis |

The visual density adapts to the current zoom level.

## 13. Opening memory details

Selecting a node opens the memory details panel.

### Timeline movement

When a user selects a node:

1. The entire timeline smoothly moves.
2. The selected node becomes centered relative to the screen.
3. The current zoom level remains unchanged.
4. A details panel opens from the left.

The selected node should remain visible behind the panel.

The timeline centers the node relative to the screen, not relative to the remaining uncovered space beside the panel.

### Selecting another node

When the panel is already open:

- Selecting another node moves the timeline to center the new node.
- The existing panel remains open.
- Its content is replaced with the newly selected memory.
- The zoom level remains unchanged.

### Closing the panel

The panel closes using its X button.

After closing:

- The timeline remains at its current position.
- The zoom level is preserved.
- The user can continue exploring from the selected memory.

## 14. Details panel

The details panel:

- Opens from the left.
- Has a fixed desktop width.
- May partially cover the timeline.
- Uses the same layout regardless of zoom level.

It displays:

- Photo gallery, if available.
- Title.
- Date.
- Description, if available.
- Participants, if available.
- Location, if available.

The panel contains an edit action represented by a pencil icon.

## 15. Editing and deleting memories

Editing is performed inside the existing details panel.

Selecting the pencil icon switches the panel into edit mode.

The panel does not open a separate window.

### Edit mode

- Fields become editable.
- User can change all memory data.
- User can add, remove and reorder photos.
- User can change participants and location.
- Save and Cancel actions appear at the bottom.

### Saving

After saving:

- The memory is updated.
- The details panel returns to read mode.
- The timeline representation updates automatically.

### Canceling

Cancel discards unsaved modifications.

### Closing with unsaved changes

If the user selects X while there are unsaved changes:

- Show a confirmation dialog.
- "Discard changes" discards modifications and closes the panel.
- "Return to editing" returns to edit mode.

If there are no unsaved changes, the panel closes immediately.

### Deleting

The delete action is available in the edit menu.

Deleting requires explicit confirmation.

After deletion:

- The node and its card disappear.
- Remaining nodes are automatically repositioned.
- The user stays at the same timeline position and zoom level.
- A short confirmation message is displayed.

## 16. Adding memories

A permanent "Add memory" button is available in the application interface.

It is separate from the plus and minus zoom controls.

Selecting it opens a modal in the center of the screen.

### Modal appearance

- The timeline remains visible in the background.
- The background becomes slightly darker.
- The background is lightly blurred.
- The modal appears above the timeline.

Closing the modal restores the exact previous timeline position and zoom level.

### Memory form

Required:

- Title.
- Date.

Optional:

- Multiple photos.
- Description.
- Participants.
- Location.

### Date rules

- Default date is today.
- Past dates are allowed.
- Future dates are not allowed.

### Photo behavior

- Multiple photos are supported.
- Users can preview photos before saving.
- Individual photos can be removed.
- Users can reorder photos.
- The first photo becomes the primary photo.

### After saving

- The new memory is inserted at its chronological position.
- If it is the newest memory, the timeline expands to the right.
- The user's current timeline position is preserved.
- The zoom level is not changed automatically.
- A short success confirmation is displayed.

## 17. Empty timeline

When a chronicle contains no memories:

- Display the timeline without nodes.
- Show a subtle illustration matching the visual identity.
- Display a short welcoming message.
- Provide an "Add first memory" button.

The button opens the standard add-memory modal.

## 18. Real-time synchronization

The timeline is shared between all members of a chronicle.

Changes made by one member should appear automatically for other connected members without refreshing the page.

### Synchronization scope

Synchronization applies only to the chronicle currently open in a given browser tab.

Different browser tabs may independently display and synchronize different chronicles.

Synchronize:

- New memories.
- Memory edits.
- Memory deletions.
- Reaction creation.
- Reaction changes.
- Reaction removal.
- Memory participant changes.
- Photo additions and removals.
- Photo ordering changes.

Participant and photo changes are synchronized as part of the corresponding memory update.

### Synchronization behavior

- Changes should appear practically immediately after successful server-side persistence.
- The backend must save changes to PostgreSQL before distributing notifications through SignalR.
- PostgreSQL remains the source of truth.
- Receiving an update must not unexpectedly change another user's timeline position or zoom level.
- Each user maintains independent timeline navigation state.

### Reconnection

When the real-time connection is lost:

- The application attempts to reconnect automatically.
- The application must verify that the server is reachable.
- After reconnection, the client requests synchronization with the backend.
- Changes missed during disconnection must be recovered.
- The timeline returns to its previous position and zoom level.

The exact synchronization recovery protocol remains an implementation decision.

### Concurrent editing

Multiple members can edit the same memory simultaneously.

Editing must not be locked when another member is editing the same memory.

Rules:

- Changes to different fields must be merged without overwriting each other.
- If multiple users change the same field, the last successfully saved change wins.
- Updates must modify only the fields that were actually changed.
- The backend must detect and handle concurrent updates.
- An older full memory version must never overwrite newer changes to unrelated fields.
- The backend must use an appropriate versioning or concurrency control mechanism to protect data integrity.
- Successfully saved changes must be distributed to other chronicle members through real-time synchronization.
- Each user's timeline position and zoom level must remain unchanged after receiving updates.

Concurrent editing must not silently discard successfully saved changes.

### Active editing indicator

If one or more members are currently editing a memory:

- Display the display name of each active editor.
- Example: "Anna is editing this memory."
- If multiple members are editing the same memory, show all their names.
- Display the information subtly.
- Do not block the details panel or the rest of the timeline.
- The indicator disappears when the user finishes editing or is no longer considered active.

The indicator is informational and does not prevent concurrent editing.

The exact mechanism for detecting and expiring active editing sessions remains an implementation decision.

## 19. Connection loss and recovery

### MVP offline limitations

The MVP does not support full offline functionality.

- Users cannot start new operations while offline.
- Unsaved changes from an operation already in progress must be preserved locally.
- The application must clearly communicate whether changes have been saved.
- After connectivity is restored, the application verifies server availability before submitting pending changes.
- The timeline is restored to its previous position and zoom level.

### Offline screen

- Display a dedicated illustration in the visual style of The Chronicles.
- Display a friendly message explaining that the connection was lost.
- Do not display the timeline while the application is offline.

Suggested message:

"Looks like we lost our connection."

Supporting message:

"Your chronicle is waiting for you to reconnect."

### Recovery

When internet connectivity returns:

1. Detect the restored network connection.
2. Attempt to reconnect to the application server.
3. Confirm that the server is reachable.
4. Synchronize pending data.
5. Restore the timeline.

The timeline must return to its previous position and zoom level.

Network connectivity alone does not guarantee that the server is reachable.

### Unsaved changes

Unsaved changes must not be silently discarded.

The application should preserve pending changes locally and clearly communicate whether they have been saved on the server.

The exact offline persistence and conflict recovery strategy must be defined during technical architecture design.

## 20. Animations and transitions

All timeline animations should be smooth, subtle and natural.

| Interaction            | Expected behavior                        |
| ---------------------- | ---------------------------------------- |
| Horizontal movement    | Smooth movement with subtle inertia      |
| Zoom                   | Smooth continuous zoom with snapping     |
| Opening details panel  | Panel slides in from the left            |
| Opening modal          | Subtle appearance with background blur   |
| Node hover             | Short, subtle enlargement                |
| Node selection         | Smooth visual highlight                  |
| Selecting another node | Timeline moves and panel content updates |
| Time slider            | Immediate, continuous timeline movement  |

Avoid excessive motion or distracting effects.

## 21. Empty and loading states

The implementation should account for:

- Empty timeline.
- Timeline loading.
- Memory loading.
- Real-time synchronization updates.
- Connection loss.
- Reconnection.
- Failed memory operations.

Loading and error states should preserve the visual identity of The Chronicles.

## 22. Performance expectations

The timeline may eventually contain a large number of memories.

The implementation should be designed with performance in mind.

Important considerations:

- Avoid rendering unnecessary elements.
- Consider rendering only visible timeline elements when appropriate.
- Keep drag and zoom interactions responsive.
- Avoid expensive recalculation of all nodes on every interaction.
- Optimize photo thumbnails.
- Keep real-time updates scoped to the relevant chronicle.

Specific performance targets and rendering architecture will be defined during technical planning.

## 23. Open implementation decisions

The following details are intentionally not fixed yet:

- Exact zoom thresholds.
- Zoom anchoring behavior.
- Exact animation durations.
- Exact dimensions of timeline nodes.
- Exact details panel dimensions.
- Exact photo gallery layout.
- Exact collision detection and positioning algorithm.
- Time slider dimensions and final placement.
- Offline storage implementation.
- Rendering and virtualization strategy.

These decisions should be discussed during architecture and implementation planning.

## 24. Non-negotiable behavior

The following rules must not be violated during implementation:

1. Real dates determine chronological order.
2. Every memory has its own node.
3. Zoom and time navigation remain independent.
4. Selecting a memory does not change the zoom level.
5. The selected node is centered relative to the screen.
6. The details panel uses the same layout at every zoom level.
7. Opening and closing panels must preserve the user's navigation state as specified.
8. Real-time updates must not unexpectedly move another user's timeline.
9. Visual collision handling must not modify actual memory dates.
10. Unsaved changes must never disappear silently.
