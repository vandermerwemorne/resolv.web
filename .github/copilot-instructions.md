# GitHub Copilot Instructions

## Project Guidelines

### JavaScript Framework
- **Always use jQuery** for DOM manipulation and event handling
- Do not use vanilla JavaScript unless specifically required
- Use jQuery's concise syntax and methods (e.g., `$()`, `.on()`, `.val()`, `.text()`)

### ASP.NET Core MVC Script Placement
- **Always place jQuery scripts in `@section Scripts` blocks** in Razor views
- This ensures jQuery is loaded before your custom scripts execute
- Prevents "$ is not defined" errors

#### Correct Pattern:
```razor
@section Scripts {
    <script>
        $(document).ready(function() {
            // Your jQuery code here
        });
    </script>
}
```

#### Incorrect Pattern (will cause errors):
```razor
<script>
    $(document).ready(function() {
        // This will fail - jQuery not loaded yet
    });
</script>
```

### Code Style
- Use meaningful variable names with jQuery prefix: `$nameInput`, `$submitButton`
- Store original form state for reset functionality
- Use event delegation and proper event handling
- Include `event.stopPropagation()` when needed to prevent event bubbling

### Form Interactions
- When implementing edit functionality:
  - Store original form state on page load
  - Toggle between create/update modes
  - Show/hide buttons appropriately (`Cancel` button hidden by default)
  - Reset forms to original state when canceling

### AJAX Patterns
- Use jQuery's `$.ajax()` method for server communication
- Handle loading, success, and error states
- Disable form elements during AJAX operations

### CSS Classes
- Use hover effects for clickable elements
- Implement visual feedback for selected states
- Follow Bootstrap conventions where applicable

## Repository Structure
- Controllers handle both create and update operations in single POST methods
- Use repository pattern with interfaces in Domain layer
- Implement update methods in Infrastructure repositories
- Models in `Resolv.Web.Models` namespace for view models