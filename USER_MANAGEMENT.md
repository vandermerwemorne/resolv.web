# User Management Flow

This document describes the new user management feature that allows administrators to add and manage users for holding companies.

## Overview

The user management flow follows the same design pattern as the existing onboarding screens, providing a consistent user experience. Users are associated with specific holding companies and their data is stored in the corresponding company schema.

## Features

### 1. User Management Dashboard (`/User`)
- **Holding Company Selection**: Dropdown to select which holding company's users to manage
- **User List**: Displays all users for the selected holding company
- **Add New User**: Quick access button to add users for the selected holding company
- **User Details**: Shows full name, known name, email, access status, roles, and creation date

### 2. User Creation and Editing (`/User/CreateUser`)
- **Form Fields**:
  - Full Name (required, max 45 characters)
  - Known Name (optional, max 45 characters) 
  - Email Address (required, validated format)
  - Password (required for new users)
  - Roles (optional, comma-separated)
  - Has Access (checkbox to enable/disable user)

- **Interactive User List**: Click on any existing user to edit their details
- **Form Modes**:
  - **Create Mode**: Default state for adding new users
  - **Edit Mode**: Activated when clicking on an existing user
  - **Cancel**: Resets form back to create mode

## Technical Implementation

### Models
- `UserManagement`: View model for the main user dashboard
- `User`: View model for user creation and editing forms

### Controller Actions
- `Index` (GET/POST): Main user management dashboard with holding company selection
- `CreateUser` (GET/POST): User creation and editing form

### Repository Integration
- Uses existing `CustUserRepository` for CRUD operations
- Users are stored in company-specific schemas (e.g., `companyx.user`)
- Follows the same schema pattern as divisions and assessment sites

## Design Consistency

The user management screens maintain consistency with existing onboarding screens:

- **Visual Style**: Same CSS classes, Bootstrap components, and hover effects
- **Form Layout**: Consistent label positioning and form structure
- **JavaScript Patterns**: Same jQuery implementations for form interactions
- **Navigation**: Breadcrumb navigation and consistent button placement
- **Validation**: ASP.NET MVC validation with client-side scripts

## Security Considerations

- Password fields use `type="password"` and `autocomplete="new-password"`
- Form validation prevents empty required fields
- User access can be disabled without deleting accounts
- Users are scoped to specific holding company schemas

## Usage Flow

1. Navigate to `/User` to access user management
2. Select a holding company from the dropdown
3. Click "Load Users" to view existing users for that company
4. Click "Add New User" or click on an existing user to edit
5. Fill out the user form with required information
6. Save to create/update the user
7. Use the breadcrumb or "Back" button to return to the user list

## Future Enhancements

- User role management with predefined roles
- Password strength validation
- User activity logging
- Bulk user operations
- User profile pictures
- Email notifications for new users