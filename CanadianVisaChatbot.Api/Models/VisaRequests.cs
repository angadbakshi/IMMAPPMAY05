namespace CanadianVisaChatbot.Api.Models;

public record StudyVisaEligibilityRequest(
    string Age,
    string Nationality,
    string Education
);

public record StudyPlanRequest(
    string Program,
    string Institution,
    string Duration,
    string CareerGoals
);

public record WorkVisaLMIARequest(
    string JobTitle,
    string Industry,
    string Location,
    string Salary
);

public record EmploymentLetterRequest(
    string Company,
    string Position,
    string StartDate,
    string Terms
);

public record SpousalRelationshipRequest(
    string Type,
    string Duration,
    string LivingSituation,
    string Communication
);

public record SponsorLetterRequest(
    string Name,
    string Duration,
    string FinancialCapacity,
    string LivingArrangements
);