# Setup and Configuration Guide

## Development Environment Setup

### 1. Prerequisites
- .NET 8 SDK
- Visual Studio 2022 or VS Code
- MAUI Workload
- Firebase CLI
- Git

### 2. Initial Setup
```bash
# Clone repository
git clone [repository-url]

# Install MAUI workload
dotnet workload install maui

# Restore packages
dotnet restore
```

## Configuration

### 1. API Configuration
```json
{
  "DeepSeek": {
    "ApiKey": "your-api-key",
    "BaseUrl": "https://api.deepseek.com/v1/"
  },
  "Firebase": {
    "ProjectId": "your-project-id"
  }
}
```

### 2. Firebase Setup
- Create Firebase project
- Download credentials
- Configure authentication
- Set up Cloud Storage

### 3. Mobile Configuration
- Update Android package name
- Configure iOS bundle identifier
- Add Firebase configuration files
- Set up debugging

## Running the Application

### 1. Start API
```bash
cd CanadianVisaChatbot.Api
dotnet run
```

### 2. Run Mobile App
```bash
cd CanadianVisaChatbot.Mobile
dotnet build -t:Run -f net8.0-ios
```

## Development Guidelines

### 1. Code Structure
- Follow MVVM pattern
- Use dependency injection
- Implement interfaces
- Document APIs

### 2. Best Practices
- Error handling
- Logging
- Security measures
- Performance optimization

### 3. Testing
- Unit tests
- Integration tests
- UI testing
- Performance testing

## Deployment

### 1. API Deployment
- Configure hosting
- Set up SSL
- Enable monitoring
- Configure scaling

### 2. Mobile Deployment
- App signing
- Store submission
- Beta testing
- Release management