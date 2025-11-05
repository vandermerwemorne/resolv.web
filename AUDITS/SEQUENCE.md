# Risk Assessment System - Technical Sequence Diagrams

## System Overview
This document provides technical sequence diagrams for the Risk Assessment System (Behemoth), a web-based application for managing occupational health and safety risk assessments. The system serves multiple client organizations and provides comprehensive risk evaluation workflows.

## User Personas

### 1. Master User (ISS Administrator)
- **Role**: System administrator from Innovative Shared Services (ISS)
- **Permissions**: Full system access, user management, all reporting capabilities
- **Key Activities**: System configuration, user management, master data maintenance

### 2. Risk Assessor 
- **Role**: Professional risk assessment specialist
- **Permissions**: Risk assessment creation/editing, hazard identification, risk evaluation
- **Key Activities**: Conducting risk assessments, evaluating controls, assigning corrective actions

### 3. Client Contact
- **Role**: Client organization representative (e.g., Nedbank, PDC)
- **Permissions**: Limited access to specific client data, corrective action updates
- **Key Activities**: Reviewing assigned risks, updating corrective action status

### 4. Reporting User
- **Role**: Management user focused on reporting and oversight
- **Permissions**: Read-only access to reports and dashboards
- **Key Activities**: Viewing reports, monitoring compliance, trend analysis

---

## 1. User Authentication Flow

```mermaid
sequenceDiagram
    participant U as User
    participant LC as LoginController
    participant UR as UserRepository
    participant HC as HoldingCompanyRepository
    participant H as Hash
    participant A as Auth
    participant LL as LoginLog
    participant LLR as LandingController

    U->>LC: POST /Login/Login(email, password)
    LC->>H: Generate(password)
    H-->>LC: hashedPassword
    
    Note over LC: Try ISS User Login (dbo.user)
    LC->>UR: Select(email, hashedPassword)
    alt ISS User Found
        UR-->>LC: UserModel with Id > 0
        LC->>LL: Capture(userId)
        LC->>A: Login(userObj)
        A->>A: Generate Claims (roles, permissions)
        LC-->>U: Redirect to Landing
    else ISS User Not Found
        UR-->>LC: UserModel with Id = 0
        Note over LC: Try Client Contact Login
        LC->>HC: SelectList()
        HC-->>LC: List of holding companies
        loop For each holding company
            LC->>LC: TryLogin(clientDict, model, holdingCompanyId)
        end
        alt Client Contact Found
            LC->>A: Login(clientUserObj, tier, company)
            LC-->>U: Redirect to appropriate dashboard
        else All logins fail
            Note over LC: Try Backdoor Login (config-based)
            alt Backdoor credentials match
                LC->>LL: Capture(-1)
                LC->>A: Login(backdoorUser)
                LC-->>U: Redirect to National
            else Authentication failed
                LC-->>U: Return login form with error
            end
        end
    end
```

---

## 2. Risk Assessment Creation and Management Flow

```mermaid
sequenceDiagram
    participant A as Risk Assessor
    participant LC as LandingController
    participant CR as ClientRepository
    participant RR as RiskRepository
    participant RC as RiskController
    participant RLR as RiskLineRepository
    participant PS as PictureSave
    participant RL as RiskListController

    Note over A: Assessor selects client and starts risk assessment
    A->>LC: GET /Landing/Index
    LC->>LC: SetSelectList(true)
    LC-->>A: Landing page with client selection

    A->>LC: LoadClient(holdingCompanyId, provinceId)
    LC->>LC: LookupsMvc.ClientByHoldingCompanyIdAndProvinceId()
    LC-->>A: Client dropdown options

    A->>LC: LoadRisksByClient(clientId)
    LC->>CR: LoadRiskSummaryByClient(clientId)
    CR-->>LC: Risk assessments for client
    alt Existing assessments found
        LC-->>A: Display existing risk assessments
        A->>LC: SelectRiskAssessment(riskId)
        LC->>RR: Select(riskId)
        LC->>CR: Select(clientId)
        LC->>LC: Set session/cookie data
        LC-->>A: Redirect to RiskList
    else No existing assessments
        LC-->>A: Option to start new assessment
        A->>A: Create new risk assessment
        Note over A: Redirect to risk creation flow
    end

    Note over A: Risk Line Creation (Hazard Identification)
    A->>RC: GET /Risk/HazardIdentification
    RC-->>A: Hazard identification form

    A->>RC: POST HazardIdentification(hazard data + picture)
    alt Picture uploaded
        RC->>PS: Save(file, metadata)
        PS->>PS: Validate file size and type
        PS-->>RC: pictureId or error message
    end
    
    RC->>RC: Validate model state
    alt New risk line
        RC->>RC: UniqueReference(riskId, referenceNo)
        alt Reference is unique
            RC->>RLR: Insert(riskLineModel)
            RLR-->>RC: newRiskLineId
            RC->>RC: SetRiskLineId(newRiskLineId)
        else Reference duplicate
            RC-->>A: Validation error
        end
    else Updating existing risk line
        RC->>RLR: Select(riskLineId)
        RC->>RLR: Update(riskLineModel)
        RC->>RC: HistoryRiskLine.Insert() // Audit trail
    end
    
    RC-->>A: Redirect to evaluation step

    Note over A: Risk Evaluation and Controls
    A->>RC: POST EvaluationOfRiskAndControls(evaluation data)
    RC->>RC: Calculate residual risk scores
    RC->>RC: Determine priority levels
    RC->>RLR: Update risk line with evaluation data
    RC-->>A: Success confirmation

    Note over A: Risk Completion and Verification
    A->>RL: Complete risk assessment
    RL->>RR: Update(riskStatusId = complete)
    RL-->>A: Assessment marked complete
```

---

## 3. Client Contact Corrective Action Flow

```mermaid
sequenceDiagram
    participant CC as Client Contact
    participant LC as LoginController
    participant A as Auth
    participant CAC as CorrectiveActionController
    participant RLR as RiskLineRepository
    participant N as Notification
    participant RU as Risk User

    Note over CC: Client contact logs in with limited permissions
    CC->>LC: POST /Login/Login(email, password)
    LC->>LC: TryLogin via client contact lookup
    alt Client contact authenticated
        LC->>A: Login(clientObj, tier, company)
        A->>A: Set limited permissions/claims
        LC-->>CC: Redirect to client dashboard
    end

    Note over CC: View assigned corrective actions
    CC->>CAC: GET /CorrectiveAction/Index
    CAC->>RLR: SelectAssignedToUser(contactId)
    RLR-->>CAC: List of assigned risk lines
    CAC-->>CC: Corrective actions dashboard

    Note over CC: Update corrective action status
    CC->>CAC: POST UpdateCorrectiveAction(actionId, status, notes)
    CAC->>RLR: Select(riskLineId)
    CAC->>RLR: Update corrective action fields
    
    alt Status changed to completed
        CAC->>N: SendNotification(assessor, completion_update)
        CAC->>RU: LogActivity(actionId, "completed")
    end
    
    CAC-->>CC: Confirmation message
```

---

## 4. Reporting and Verification Flow

```mermaid
sequenceDiagram
    participant RU as Reporting User
    participant VC as VerifyController
    participant VLC as VerifyListController
    participant RR as ReportRepository
    participant RLR as RiskLineRepository
    participant DR as DashboardRepository
    participant PDF as CreatePDF

    Note over RU: Access verification and reporting functions
    RU->>VLC: GET /VerifyList/Index
    VLC->>RLR: SelectForVerification()
    RLR-->>VLC: Risk assessments pending verification
    VLC-->>RU: Verification queue

    Note over RU: Review specific risk assessment
    RU->>VC: GET /Verify/Details(riskId)
    VC->>RR: Select(riskId)
    VC->>RLR: SelectByRiskId(riskId)
    VC-->>RU: Detailed risk assessment view

    alt Approve assessment
        RU->>VC: POST Approve(riskId)
        VC->>RR: Update(riskStatusId = approved)
        VC->>VC: LogVerificationAction()
        VC-->>RU: Approval confirmation
    else Request changes
        RU->>VC: POST RequestChanges(riskId, comments)
        VC->>RR: Update(riskStatusId = revision_required)
        VC->>N: NotifyAssessor(assessorId, revision_request)
        VC-->>RU: Revision request sent
    end

    Note over RU: Generate and view reports
    RU->>RR: GET /Report/Dashboard
    RR->>DR: GetDashboardData(filters)
    DR-->>RR: Aggregated statistics
    RR-->>RU: Dashboard with KPIs

    RU->>RR: GET /Report/DetailedReport(parameters)
    RR->>RLR: SelectByFilters(parameters)
    RLR-->>RR: Filtered risk data
    alt PDF generation requested
        RR->>PDF: GenerateReport(data, template)
        PDF-->>RR: PDF file
        RR-->>RU: Download PDF report
    else Web view
        RR-->>RU: HTML report view
    end
```

---

## 5. Site Visit Assessment Flow

```mermaid
sequenceDiagram
    participant A as Risk Assessor
    participant SVC as SiteVisitController
    participant SVR as SiteVisitRepository
    participant SVQR as SiteVisitQuestionRepository
    participant SVCR as SiteVisitCampusRepository
    participant PS as PictureSave

    Note over A: Conduct site visit assessment
    A->>SVC: GET /SiteVisit/Create(clientId)
    SVC->>SVCR: GetCampusByClient(clientId)
    SVC->>SVQR: GetQuestionTemplate()
    SVC-->>A: Site visit form with questions

    A->>SVC: POST Create(siteVisitData)
    SVC->>SVR: Insert(siteVisitModel)
    SVR-->>SVC: newSiteVisitId

    Note over A: Answer questions and capture evidence
    loop For each question category
        A->>SVC: POST AnswerQuestion(questionId, answer, photo)
        alt Photo provided
            SVC->>PS: Save(photo, siteVisitId, questionId)
            PS-->>SVC: photoId
        end
        SVC->>SVQR: SaveAnswer(siteVisitId, questionId, answer, photoId)
    end

    Note over A: Complete site visit
    A->>SVC: POST Complete(siteVisitId)
    SVC->>SVR: Update(status = complete, completionDate)
    SVC->>SVC: CalculateComplianceScore()
    SVC-->>A: Site visit completed with score

    Note over A: Generate site visit report
    A->>SVC: GET /SiteVisit/Report(siteVisitId)
    SVC->>SVR: SelectWithAnswers(siteVisitId)
    SVC->>PDF: GenerateSiteVisitReport(data)
    PDF-->>SVC: PDF report
    SVC-->>A: Download site visit report
```

---

## 6. System Administration Flow

```mermaid
sequenceDiagram
    participant MU as Master User
    participant UC as UserController
    participant UR as UserRepository
    participant HC as HoldingCompanyController
    participant HCR as HoldingCompanyRepository
    participant CR as ClientRepository
    participant SC as SystemController

    Note over MU: User management operations
    MU->>UC: GET /User/Index
    UC->>UR: SelectList()
    UR-->>UC: List of all users
    UC-->>MU: User management dashboard

    MU->>UC: GET /User/Edit(userId)
    UC->>UR: Select(userId)
    UC-->>MU: User edit form

    MU->>UC: POST Edit(userModel)
    UC->>UR: Update(userModel)
    UC->>UC: LogUserChange() // Audit trail
    UC-->>MU: User updated successfully

    Note over MU: Client organization management
    MU->>HC: GET /HoldingCompany/Index
    HC->>HCR: SelectList()
    HCR-->>HC: List of holding companies
    HC-->>MU: Organization management view

    MU->>HC: POST CreateClient(clientData)
    HC->>CR: Insert(clientModel)
    HC->>HCR: LinkClientToHoldingCompany()
    HC-->>MU: Client created successfully

    Note over MU: System configuration
    MU->>SC: GET /System/Configuration
    SC->>SC: LoadSystemSettings()
    SC-->>MU: System configuration panel

    MU->>SC: POST UpdateConfiguration(settings)
    SC->>SC: ValidateSettings()
    SC->>SC: SaveToConfigFile()
    SC-->>MU: Configuration updated
```

---

## 7. Risk Re-evaluation Flow

```mermaid
sequenceDiagram
    participant A as Risk Assessor
    participant REL as ReEvalListController
    participant RER as ReEvalRepository
    participant REC as ReEvalController
    participant RLR as RiskLineRepository
    participant PS as PictureSave
    participant N as Notification
    participant CAR as CorrectiveActionRepository

    Note over A: Re-evaluate existing risk assessments
    A->>REL: GET /ReEvalList/Index
    REL->>RER: SelectForReEvaluation()
    RER-->>REL: Risk lines due for re-evaluation
    REL-->>A: Re-evaluation queue

    A->>REC: GET /ReEval/Index(riskLineId)
    REC->>RLR: Select(riskLineId)
    REC->>REC: CreateViewModel(riskLineId)
    REC-->>A: Re-evaluation form with current data

    Note over A: Update corrective actions and evidence
    A->>REC: POST Save(reEvalData + evidence photos)
    alt Evidence photos provided
        REC->>PS: Save(photos, reEvalId)
        PS-->>REC: photoIds
    end
    
    REC->>RER: Update(reEvalModel)
    REC->>CAR: UpdateCorrectiveActions(actions)
    
    alt Re-evaluation complete
        REC->>RLR: Update(status = "Re-evaluation Complete")
        REC->>N: NotifyStakeholders(completion)
    else Requires further action
        REC->>N: NotifyAssignedPersons(actionRequired)
    end
    
    REC-->>A: Re-evaluation saved successfully
```

---

## 8. Scheduling and Calendar Management Flow

```mermaid
sequenceDiagram
    participant PM as Project Manager
    participant SC as SchedulingController
    participant SR as ScheduleRepository
    participant CR as ClientRepository
    participant RR as RiskRepository
    participant ER as ErgonomicRepository
    participant N as NotificationService

    Note over PM: Manage assessment scheduling
    PM->>SC: GET /Scheduling/Index
    SC->>SR: SelectList()
    SC->>CR: GetClientDetails()
    SC->>RR: GetLastAssessmentDates()
    SC-->>PM: Scheduling dashboard with client timelines

    Note over PM: Schedule new assessment
    PM->>SC: POST CreateSchedule(clientId, assessmentType, scheduledDate)
    SC->>SR: Insert(scheduleModel)
    SC->>CR: Select(clientId)
    
    alt Ergonomic Assessment
        SC->>ER: CreateErgonomicSchedule(scheduleId)
    else Standard Risk Assessment
        SC->>SC: CreateRiskAssessmentSchedule(scheduleId)
    end
    
    SC->>N: SendScheduleNotification(assessor, client, date)
    SC-->>PM: Schedule created successfully

    Note over PM: Generate scheduling reports
    PM->>SC: GET ExportSchedule(filters)
    SC->>SR: SelectByFilters(dateRange, region, type)
    SC->>SC: GenerateExcelReport(scheduleData)
    SC-->>PM: Download Excel schedule
```

---

## 9. National/Regional Dashboard Flow

```mermaid
sequenceDiagram
    participant MU as Management User
    participant NC as NationalController
    participant PC as ProvincialController
    participant DR as DashboardRepository
    participant RR as ReportRepository
    participant CR as ClientRepository
    participant RegR as RegionRepository

    Note over MU: Access high-level analytics
    MU->>NC: GET /National/Index
    NC->>NC: SetSelectList()
    NC-->>MU: National dashboard with region filters

    MU->>NC: LoadStats(holdingCompanyId, siteType)
    NC->>DR: SitesAssessed(holdingCompanyId, siteType)
    NC->>DR: RiskLines(holdingCompanyId, siteType)
    NC->>DR: ReEvaluations(holdingCompanyId, siteType)
    NC->>NC: CalculateKPIs(rawData)
    NC-->>MU: National statistics summary

    Note over MU: Drill down to regional view
    MU->>PC: GET /Provincial/Index(regionId)
    PC->>RegR: SelectByRegion(regionId)
    PC->>DR: GetRegionalStats(regionId)
    PC-->>MU: Regional dashboard

    Note over MU: Filter by sub-region
    MU->>NC: GetStats_SubRegion(holdingCompanyId, siteType, subRegionId)
    NC->>DR: SitesAssessedBySubRegion(subRegionId)
    NC->>DR: RiskLinesBySubRegion(subRegionId)
    NC-->>MU: Sub-regional statistics

    Note over MU: Export dashboard data
    MU->>NC: ExportDashboard(filters)
    NC->>RR: GenerateDashboardReport(parameters)
    RR-->>NC: Dashboard report
    NC-->>MU: Download report file
```

---

## 10. Action Plan Reporting Flow

```mermaid
sequenceDiagram
    participant RM as Reporting Manager
    participant ARC as ActionReportController
    participant RR as ReportRepository
    participant RUR as RiskUserRepository
    participant ALR as ActionLogRepository
    participant CAR as CorrectiveActionRepository

    Note over RM: Generate action plan reports
    RM->>ARC: GET /ActionReport/Index
    ARC->>ARC: SetSelectList()
    ARC-->>RM: Action report filter form

    RM->>ARC: POST Index(filters)
    ARC->>ALR: Insert(reportLogStart)
    ARC->>RUR: SelectByEmail(currentUser)
    
    alt Current date report
        ARC->>RR: SelectListActionPlan(filters)
    else Historical report
        ARC->>RR: SelectListActionPlan_LogData(date, filters)
    end
    
    Note over ARC: Apply designation-based filtering
    ARC->>ARC: FilterByUserDesignation(data, userDesignation)
    
    ARC->>ARC: BuildData(filteredResults)
    ARC->>ARC: SaveReport(reportData, filters)
    ARC->>ALR: Insert(reportLogComplete)
    ARC-->>RM: Action plan report generated

    Note over RM: Export to Excel
    RM->>ARC: GET ExportExcel(reportId)
    ARC->>ARC: GenerateExcelFile(reportData)
    ARC-->>RM: Download Excel file
```

---

## 11. Training and Compliance Flow

```mermaid
sequenceDiagram
    participant TU as Training User
    participant TC as TrainingController
    participant ALR as ActionLogRepository
    participant TR as TrainingRepository
    participant UR as UserRepository
    participant CR as CertificationRepository

    Note over TU: Access training modules
    TU->>TC: GET /Training/Index
    TC->>ALR: Insert(accessLog)
    TC-->>TU: Training dashboard

    Note over TU: Complete training module
    TU->>TC: POST CompleteModule(moduleId)
    TC->>TR: RecordCompletion(userId, moduleId)
    TC->>CR: UpdateCertification(userId, moduleId)
    TC-->>TU: Training completion confirmation

    Note over TU: View training history
    TU->>TC: GET TrainingHistory(userId)
    TC->>TR: SelectUserTraining(userId)
    TC->>CR: SelectUserCertifications(userId)
    TC-->>TU: Training history and certifications
```

---

## 12. Note and Documentation Management Flow

```mermaid
sequenceDiagram
    participant U as User
    participant NC as NoteController
    participant NR as NoteRepository
    participant RLR as RiskLineRepository
    participant A as AuditLogger

    Note over U: Add notes to risk assessments
    U->>NC: GET /Note/Create(riskLineId)
    NC->>RLR: Select(riskLineId)
    NC-->>U: Note creation form

    U->>NC: POST Create(noteData)
    NC->>NR: Insert(noteModel)
    NC->>A: LogNoteCreation(noteId, userId)
    NC-->>U: Note saved successfully

    Note over U: View note history
    U->>NC: GET /Note/History(riskLineId)
    NC->>NR: SelectByRiskLine(riskLineId)
    NC-->>U: Chronological note history

    Note over U: Update existing note
    U->>NC: POST Update(noteId, updatedContent)
    NC->>NR: Update(noteModel)
    NC->>A: LogNoteModification(noteId, userId)
    NC-->>U: Note updated successfully
```

---

## Controller Coverage Analysis

### **Covered Controllers (Primary User Journeys)**
The sequence diagrams above cover the following major controller groups:

#### Authentication & User Management
- `LoginController` - User authentication flow
- `UserController` - User administration
- `RiskUserController` - Risk user management
- `Auth` (Common) - Claims-based authentication

#### Core Risk Assessment
- `LandingController` - Client selection and risk initiation
- `RiskController` - Hazard identification and risk evaluation
- `RiskListController` - Risk assessment overview and management
- `ReEvalController` - Risk re-evaluation process
- `ReEvalListController` - Re-evaluation queue management

#### Site Visits & Assessments
- `SiteVisitController` - Site visit assessments
- `SiteVisitQuestionController` - Question management
- `SiteVisitReportController` - Site visit reporting
- `SiteVisitSummaryReportController` - Summary reporting

#### Verification & Approval
- `VerifyController` - Risk assessment verification
- `VerifyListController` - Verification queue
- `VerifyFilterController` - Verification filtering

#### Reporting & Analytics
- `NationalController` - National-level dashboards
- `ProvincialController` - Provincial-level dashboards
- `ActionReportController` - Action plan reporting
- `ReportingController` - General reporting functions
- `DashboardRepository` - Dashboard data aggregation

#### Scheduling & Planning
- `SchedulingController` - Assessment scheduling
- `TrainingController` - Training management
- `NoteController` - Documentation and notes

### **Specialized Controllers (Secondary Functions)**
The following controllers handle specific business functions but follow similar patterns:

#### Reporting Variants
- `PriorityTrendAnalysisReportController` - Trend analysis
- `ProgressReportController` - Progress tracking
- `OverdueReportController` - Overdue items
- `MonthEndStatsSummaryController` - Monthly statistics
- `StatsController` / `StatsYTDController` - Statistical reporting
- `ReportBIController` - Business intelligence
- `NewImageBranchMonthlyReportController` - Branch reporting

#### Data Management
- `CampusController` - Campus/location management
- `RegionController` / `SubRegionController` - Geographic hierarchy
- `SectorXrefStepInOperationController` - Sector-operation mapping
- `UserClientController` - User-client relationships
- `OutstandingRiskDocumentController` - Document management

#### Specialized Assessments
- `ErgonomicController` - Ergonomic assessments
- `HulaminController` - Client-specific implementation
- `OtherSiteVisitController` - Alternative site visit types
- `SiteSpecificController` - Site-specific configurations

#### System Functions
- `HomeController` - Landing pages and navigation
- `ManageController` - System management
- `RegistrationController` - User registration
- `SoftwareController` - Software-specific functions
- `MethodologyController` - Assessment methodologies
- `MessageController` - System messaging

### **Pattern Summary**
Most controllers follow these common patterns already documented:

1. **CRUD Operations**: Create, Read, Update, Delete for entities
2. **Filter/Search**: Filtering and searching capabilities
3. **Reporting**: Data extraction and report generation
4. **Authorization**: Role-based access control
5. **Audit Logging**: Activity tracking and compliance

### **Missing Critical Flows (Now Added)**
The updated sequence diagrams now include:

- ✅ **Re-evaluation Process** - Critical for ongoing risk management
- ✅ **Scheduling System** - Essential for project management
- ✅ **National/Regional Dashboards** - Key for management oversight
- ✅ **Action Plan Reporting** - Critical for compliance tracking
- ✅ **Training Management** - Important for user competency
- ✅ **Note Management** - Essential for documentation

### **Comprehensive Coverage Achieved**
The sequence diagrams now provide comprehensive coverage of all major user journeys in the Risk Assessment System, representing the core workflows that drive business value and user engagement.

---

## Technical Notes

### Authentication Architecture
- The system supports multiple authentication paths:
  1. **ISS Users**: Stored in `dbo.user` table with role-based permissions
  2. **Client Contacts**: Stored in client-specific tables, limited permissions
  3. **Backdoor Access**: Configuration-based emergency access

### Role-Based Security
- **Claims-based authentication** using ASP.NET Identity
- **Hierarchical permissions**: Master users inherit all lower-level permissions
- **Client isolation**: Client contacts can only access their organization's data

### Data Flow Patterns
- **Repository Pattern**: Consistent data access layer across all entities
- **Audit Trail**: All significant changes are logged for compliance
- **Session Management**: Risk context maintained through cookies and session state
- **File Management**: Structured approach to image/document storage with validation

### Key Business Rules
1. **Risk Reference Uniqueness**: Each risk line must have a unique reference within a risk assessment
2. **Sequential Workflow**: Risk assessments follow a defined sequence (Identification → Evaluation → Controls → Completion)
3. **Client Data Isolation**: Each client organization's data is strictly segregated
4. **Approval Workflow**: Completed assessments require verification before finalization

### Integration Points
- **Crystal Reports**: For formal report generation
- **Email Notifications**: Automated alerts for assignment and completion
- **File Storage**: Secure handling of uploaded images and documents
- **Audit Logging**: Comprehensive change tracking for compliance

---

*Generated: November 6, 2025*
*System: Behemoth Risk Assessment Platform*