namespace CanadianVisaChatbot.Shared.AI.Models;

public static class VisaPrompts
{
    private const string BasePrompt = @"You are a knowledgeable Canadian immigration assistant. Provide clear, accurate, and helpful information about Canadian visa requirements and processes. Base your responses on official Canadian immigration policies.";

    public static class StudyVisa
    {
        public static string AssessEligibility(Dictionary<string, string> userData) =>
            $@"{BasePrompt}

Based on the following applicant information, assess their eligibility for a Canadian study permit and provide detailed recommendations:

Applicant Details:
- Age: {userData.GetValueOrDefault("age")}
- Nationality: {userData.GetValueOrDefault("nationality")}
- Education Level: {userData.GetValueOrDefault("education")}

Please provide:
1. Initial eligibility assessment
2. Key requirements they need to meet
3. Required documents
4. Next steps in the application process
5. Potential challenges or areas needing attention";

        public static string GenerateStudyPlan(Dictionary<string, string> programDetails) =>
            $@"{BasePrompt}

Help create a study plan for a Canadian study permit application with these details:

Program Information:
- Program: {programDetails.GetValueOrDefault("program")}
- Institution: {programDetails.GetValueOrDefault("institution")}
- Duration: {programDetails.GetValueOrDefault("duration")}
- Career Goals: {programDetails.GetValueOrDefault("careerGoals")}

Please provide:
1. Academic objectives
2. Course structure overview
3. Timeline for completion
4. Post-graduation plans
5. How this aligns with career goals";
    }

    public static class WorkVisa
    {
        public static string AssessLMIA(Dictionary<string, string> jobDetails) =>
            $@"{BasePrompt}

Analyze the Labour Market Impact Assessment (LMIA) requirements for:

Job Details:
- Title: {jobDetails.GetValueOrDefault("jobTitle")}
- Industry: {jobDetails.GetValueOrDefault("industry")}
- Location: {jobDetails.GetValueOrDefault("location")}
- Salary: {jobDetails.GetValueOrDefault("salary")}

Please provide:
1. LMIA requirement assessment
2. Wage and labor market analysis
3. Required documentation
4. Processing timeline
5. Alternative options if applicable";

        public static string GenerateEmploymentLetter(Dictionary<string, string> employmentDetails) =>
            $@"{BasePrompt}

Create an employment letter template for a work permit application:

Employment Details:
- Company: {employmentDetails.GetValueOrDefault("company")}
- Position: {employmentDetails.GetValueOrDefault("position")}
- Start Date: {employmentDetails.GetValueOrDefault("startDate")}
- Terms: {employmentDetails.GetValueOrDefault("terms")}

The letter should include:
1. Job description and responsibilities
2. Employment terms and conditions
3. Salary and benefits
4. Contract duration
5. Company commitment to immigration process";
    }

    public static class SpousalVisa
    {
        public static string AssessRelationship(Dictionary<string, string> relationshipDetails) =>
            $@"{BasePrompt}

Evaluate the relationship details for a spousal sponsorship application:

Relationship Information:
- Type: {relationshipDetails.GetValueOrDefault("type")}
- Duration: {relationshipDetails.GetValueOrDefault("duration")}
- Living Situation: {relationshipDetails.GetValueOrDefault("livingSituation")}
- Communication: {relationshipDetails.GetValueOrDefault("communication")}

Please assess:
1. Relationship eligibility
2. Required documentation
3. Potential red flags
4. Processing timeline
5. Recommendations for strengthening the application";

        public static string GenerateSponsorLetter(Dictionary<string, string> sponsorDetails) =>
            $@"{BasePrompt}

Help create a sponsorship letter template:

Sponsor Information:
- Name: {sponsorDetails.GetValueOrDefault("name")}
- Duration: {sponsorDetails.GetValueOrDefault("duration")}
- Financial Capacity: {sponsorDetails.GetValueOrDefault("financialCapacity")}
- Living Arrangements: {sponsorDetails.GetValueOrDefault("livingArrangements")}

Include:
1. Description of relationship
2. Financial commitment details
3. Living arrangement plans
4. Support system details
5. Future plans together";
    }
}