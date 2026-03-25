# 📋 SurveyApp — Anket Yönetim Sistemi

Mikroservis mimarisi üzerine inşa edilmiş, kurumsal düzeyde bir anket yönetim sistemi.

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/)
[![React](https://img.shields.io/badge/React-18-61DAFB)](https://reactjs.org/)
[![Docker](https://img.shields.io/badge/Docker-Compose-2496ED)](https://www.docker.com/)
[![RabbitMQ](https://img.shields.io/badge/RabbitMQ-MassTransit-FF6600)](https://www.rabbitmq.com/)

---

## 🏗️ Mimari

```
┌─────────────────────────────────────────────────────────┐
│                     React Frontend                       │
│                  (Vite + Redux Toolkit)                  │
└──────┬──────────┬──────────┬───────────────┬────────────┘
       │          │          │               │
       ▼          ▼          ▼               ▼
┌──────────┐ ┌─────────┐ ┌────────┐ ┌────────────────┐
│ Identity │ │ Survey  │ │Survey  │ │   Reporting    │
│ Service  │ │Mgmt     │ │Follow  │ │   Service      │
│ :7220    │ │:7158    │ │:7138   │ │   :7025        │
└──────────┘ └─────────┘ └───┬────┘ └───────┬────────┘
                              │    RabbitMQ  │
                              └──────────────┘
                         ┌────────────────────┐
                         │     PostgreSQL      │
                         │  4 ayrı veritabanı  │
                         └────────────────────┘
```

### Servisler

| Servis | Sorumluluk | Port |
|--------|-----------|------|
| **IdentityService** | JWT kimlik doğrulama, kullanıcı yönetimi | 7220 |
| **SurveyManagement** | Anket, soru, şablon CRUD işlemleri | 7158 |
| **SurveyFollow** | Anket katılımı, cevap kayıt | 7138 |
| **ReportingService** | İstatistik ve raporlama (event-driven) | 7025 |

---

## 🚀 Teknolojiler

**Backend**
- .NET 8 — Clean Architecture
- Entity Framework Core + PostgreSQL (Npgsql)
- Autofac — Dependency Injection
- MassTransit + RabbitMQ — Event-Driven Messaging
- FluentValidation
- JWT Bearer Authentication

**Frontend**
- React 18 + TypeScript
- Redux Toolkit — State Management
- Vite
- Axios

**Infrastructure**
- Docker + Docker Compose
- PostgreSQL 16
- RabbitMQ 3 (Management UI)
- Nginx (Frontend SPA)

---

## ⚡ Hızlı Başlangıç

### Docker ile (Önerilen)

```bash
git clone https://github.com/yacnuzun/SurveyApp.git
cd SurveyApp
docker-compose up --build
```

Uygulama ayağa kalktıktan sonra:

| URL | Açıklama |
|-----|---------|
| http://localhost | Frontend |
| http://localhost:15672 | RabbitMQ Management UI |
| http://localhost:7220/swagger | IdentityService API |
| http://localhost:7158/swagger | SurveyManagement API |
| http://localhost:7138/swagger | SurveyFollow API |
| http://localhost:7025/swagger | ReportingService API |

### Test Kullanıcıları

| Rol | E-posta | Şifre |
|-----|---------|-------|
| Admin | admin@surveyapp.com | Admin123! |
| Kullanıcı 1 | user1@surveyapp.com | User123! |
| Kullanıcı 2 | user2@surveyapp.com | User123! |
| Kullanıcı 3 | user3@surveyapp.com | User123! |

---

## 📁 Proje Yapısı

```
SurveyApp/
├── docker-compose.yml
├── init-db.sql
├── backend/
│   ├── services/
│   │   ├── SurveyApp.IdentityService/
│   │   ├── SurveyApp.SurveyManagement/
│   │   ├── SurveyApp.SurveyFollow/
│   │   └── SurveyApp.ReportingService/
│   └── shared/
│       └── SurveyApp.Shared/          # Ortak kütüphane
└── frontend/
    └── src/
        ├── store/                     # Redux slices
        ├── pages/                     # Sayfa bileşenleri
        ├── components/                # Ortak bileşenler
        ├── types/                     # TypeScript tipleri
        └── api/                       # Axios konfigürasyonu
```

---

## 🔄 Event-Driven Akış

```
Kullanıcı anketi doldurur
        │
        ▼
SurveyFollow → RabbitMQ → ReportingService
                               │
                               ▼
                    SurveyStatistic DB'ye kaydedilir
                               │
                               ▼
                    GET /api/Reports/{surveyId}
```

---

## 🔐 Yetkilendirme

| İşlem | Admin | Kullanıcı |
|-------|-------|-----------|
| Şablon CRUD | ✅ | ❌ |
| Soru CRUD | ✅ | ❌ |
| Anket CRUD | ✅ | ❌ |
| Anket Doldurma | ✅ | ✅ |
| Raporları Görme | ✅ | ❌ |

---

## 🏛️ Clean Architecture Katmanları

Her mikroservis aynı katman yapısını takip eder:

```
├── Domain/          # Entity'ler, enum'lar
├── Application/     # Manager'lar, DTO'lar, interface'ler
├── Infrastructure/  # Repository, DbContext, dış servisler
└── WebApi/    # Controller'lar, Program.cs
```

---

## 📊 Veritabanları

| Veritabanı | Servis | İçerik |
|-----------|--------|--------|
| surveyapp_identity | IdentityService | Users, OperationClaims |
| surveyapp_management | SurveyManagement | Surveys, Questions, Templates |
| surveyapp_follow | SurveyFollow | Participations, Answers |
| surveyapp_reporting | ReportingService | SurveyStatistics |

---

## 👤 Geliştirici

**Yalçın** — [github.com/yacnuzun](https://github.com/yacnuzun)