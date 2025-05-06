# System Architecture Overview

## High-Level Architecture

### 1. Mobile Application (MAUI)
```
CanadianVisaChatbot.Mobile/
├── ✅ Views/
│   ├── ✅ Dashboard
│   ├── ✅ DocumentManager
│   └── ✅ ApplicationProgress
├── ✅ ViewModels/
│   ├── ✅ Application Logic
│   └── ✅ State Management
└── ✅ Services/
    ├── ✅ API Integration
    └── ✅ Local Storage
```

### 2. Backend API (.NET 8)
```
CanadianVisaChatbot.Api/
├── ✅ Controllers/
│   ├── ✅ Visa Processing
│   ├── ✅ Document Management
│   └── ✅ Status Updates
├── ✅ Services/
│   ├── ✅ AI Integration
│   └── ✅ Document Processing
└── ✅ Security/
    └── ✅ Firebase Auth
```

### 3. Shared Library
```
CanadianVisaChatbot.Shared/
├── ✅ Models/
│   ├── ✅ Visa Applications
│   └── ✅ Documents
├── ✅ Services/
│   ├── ✅ DeepSeek Integration
│   └── ✅ Processing Logic
└── ✅ Utilities/
    └── ✅ Common Functions
```

## Key Components

### 1. Data Flow
```mermaid
graph TD
    A[Mobile App] --> B[API Layer]
    B --> C[AI Processing]
    C --> D[Document Storage]
    D --> B
    B --> A
```

### 2. Integration Points
- ✅ DeepSeek AI API
- ✅ Firebase Authentication
- ✅ Document Storage
- External Services

### 3. Security Measures
- ✅ JWT Authentication
- ✅ API Key Management
- ✅ Secure Storage
- ✅ Data Encryption

## Technical Stack

### 1. Frontend
- .NET MAUI
- Syncfusion Controls
- Local Storage
- Firebase SDK

### 2. Backend
- ASP.NET Core
- Entity Framework
- Firebase Admin
- Azure Services

### 3. Infrastructure
- Cloud Hosting
- CDN Integration
- Load Balancing
- Monitoring Tools