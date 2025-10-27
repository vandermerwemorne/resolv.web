using System.ComponentModel.DataAnnotations;

namespace Resolv.Web.Models;

public class UserManagement
{
    public Guid? SelectedHoldingCompanyUid { get; set; }
}

public class User
{
    public Guid? Id { get; set; }
    public Guid HoldingCompanyUid { get; set; }
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Full Name is required")]
    [Display(Name = "Full Name")]
    [StringLength(45, ErrorMessage = "Full Name cannot exceed 45 characters")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email Address is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    [Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "User has system access")]
    public bool HasAccess { get; set; } = true;

    [Display(Name = "Assessment Site Access")]
    [StringLength(200, ErrorMessage = "Assessment Site Access cannot exceed 200 characters")]
    public string AssessmentSiteAccess { get; set; } = string.Empty;

    [Display(Name = "Reset Password")]
    public bool ResetPassword { get; set; } = false;

    [StringLength(500, ErrorMessage = "Roles cannot exceed 500 characters")]
    public string Roles { get; set; } = string.Empty;

    [Display(Name = "Known Name")]
    [StringLength(45, ErrorMessage = "Known Name cannot exceed 45 characters")]
    public string KnownName { get; set; } = string.Empty;

    /// <summary>
    /// List of assessment site IDs that the user has access to
    /// </summary>
    public List<int> SelectedAssessmentSiteIds { get; set; } = new List<int>();
}

public class UserAccessAssessmentSite
{
    public int Id { get; set; }
    public string SiteName { get; set; } = string.Empty;
}

public class AdminUser
{
    public Guid? Id { get; set; }

    [Required(ErrorMessage = "Full Name is required")]
    [Display(Name = "Full Name")]
    [StringLength(45, ErrorMessage = "Full Name cannot exceed 45 characters")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email Address is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    [Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "User has system access")]
    public bool HasAccess { get; set; } = true;

    [Display(Name = "Reset Password")]
    public bool ResetPassword { get; set; } = false;

    [StringLength(500, ErrorMessage = "Roles cannot exceed 500 characters")]
    public string Roles { get; set; } = string.Empty;

    [Display(Name = "Known Name")]
    [StringLength(45, ErrorMessage = "Known Name cannot exceed 45 characters")]
    public string KnownName { get; set; } = string.Empty;
}