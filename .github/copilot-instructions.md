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

### Form Validation
- **Always use ASP.NET MVC validation** instead of custom jQuery validation
- Add `[Required]`, `[EmailAddress]`, `[StringLength]` and other data annotations to models
- Include `<partial name="_ValidationScriptsPartial" />` for client-side validation
- **NEVER use `alert()` boxes** - they are blocked by modern browsers
- Remove `novalidate` attribute from forms to enable HTML5 validation
- Use `<span asp-validation-for="PropertyName">` for field-level error display
- Use `<div asp-validation-summary="All">` for form-level error summary
- **Show validation summary conditionally** to prevent empty error containers on page load

#### Validation Pattern:
```csharp
// Model with validation attributes
public class User
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }
}
```

```razor
<!-- View with conditional validation summary -->
<form asp-action="Create" method="post">
    <div asp-validation-summary="All" class="alert alert-danger"
         style="display: @(ViewData.ModelState.IsValid ? "none" : "block")"></div>
    
    <input asp-for="Name" class="form-control" />
    <span asp-validation-for="Name" class="text-danger"></span>
    
    <partial name="_ValidationScriptsPartial" />
</form>
```

### Dropdown Lists
- **Always use modern ASP.NET Core tag helpers** for dropdown lists
- Use `asp-for` and `asp-items` instead of legacy `@Html.DropDownListFor`
- Include default option text within the select element
- Use meaningful IDs for JavaScript interaction

#### Correct Pattern (Modern ASP.NET Core):
```razor
<select asp-for="SelectedItemId" asp-items="Model.Items" 
        class="form-select" id="itemSelect">
    <option value="">-- Select Item --</option>
</select>
```

#### Incorrect Pattern (Legacy MVC 5 syntax):
```razor
@Html.DropDownListFor(m => m.SelectedItemId, 
    Model.Items, 
    new { @class = "form-select", @id = "itemSelect" })
```

#### Benefits of Modern Syntax:
- Better IntelliSense and compile-time checking
- Cleaner, more readable code
- Better integration with model binding and validation
- Follows current ASP.NET Core best practices

### Partial Views
- **Use partial views for dynamic content** instead of building HTML in JavaScript
- Prefer server-side rendering with Razor over client-side string concatenation
- Return `PartialView()` from controller actions for AJAX-loaded content
- Place reusable UI components in `Views/Shared/` or controller-specific `Views/[Controller]/` folders

#### Benefits of Partial Views:
- Strongly-typed models with IntelliSense
- Automatic HTML encoding for security
- Better maintainability and reusability
- Server-side validation and formatting
- Separation of concerns (view logic in views, not JavaScript)

#### When to Use Partial Views vs AJAX:
- **Partial Views**: Dynamic content with complex HTML structure, tables, forms
- **AJAX with JSON**: Simple data updates, dropdown population, validation responses

### Form Interactions
- When implementing edit functionality:
  - Store original form state on page load
  - Toggle between create/update modes
  - Show/hide buttons appropriately (`Cancel` button hidden by default)
  - Reset forms using `form[0].reset()` instead of manual field clearing

### AJAX Patterns
- Use jQuery's `$.ajax()` method for server communication
- Handle loading, success, and error states
- Disable form elements during AJAX operations
- Display server errors in validation summary, not alert boxes
- **For complex HTML content**: Return partial views instead of building HTML strings in JavaScript
- **For simple data**: Continue using JSON responses (dropdowns, validation, status updates)

### CSS Classes
- Use hover effects for clickable elements
- Implement visual feedback for selected states
- Follow Bootstrap conventions where applicable

## Repository Structure
- Controllers handle both create and update operations in single POST methods
- Use repository pattern with interfaces in Domain layer
- Implement update methods in Infrastructure repositories
- Models in `Resolv.Web.Models` namespace for view models