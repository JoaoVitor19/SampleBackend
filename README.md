## 🇺🇸 `README.md` — Portuguese Version

# 🔐 Auth API .NET 9

API de autenticação simples usando .NET 9.  
Conta com autenticação JWT, autenticação em duas etapas (2FA), Entity Framework Core e middleware global de tratamento de exceções.

---

## ⚙️ Tecnologias

- [.NET 9](https://dotnet.microsoft.com)
- Entity Framework Core
- JWT Bearer (`Microsoft.AspNetCore.Authentication.JwtBearer`)
- Two Factor Auth com [Otp.NET](https://github.com/kspearrin/Otp.NET)
- Middleware customizado para exceções globais
- Modelo de usuário e migration já gerados

---

## 🚀 Como rodar

1. **Clone o repositório**
   ```bash
   git clone https://github.com/seu-usuario/seu-repo.git
   cd seu-repo

2. **Configure o appsettings.json**
   ```bash
   {
     "ConnectionStrings": {
     "DefaultConnection": "Username=;Password=;Host=;Port=5432;Database=;",
     "RedisConnection": "localhost:6379,password="
     },
     "Jwt": {
       "Secret": "PROVIDE YOUR SECRET TOKEN HERE",
       "Issuer": "Issuer",
       "Audience": "Audience",
       "ExpiresIn": "24"
     }
   }
3. **Rode o Projeto**
   ```bash
   dotnet restore
   dotnet run

## 🇺🇸 `README.md` — English Version

# 🔐 Auth API .NET 9

A simple authentication API using .NET 9.  
Includes JWT authentication, two-factor authentication (2FA), Entity Framework Core, and global exception handling middleware.

---

## ⚙️ Technologies

- [.NET 9](https://dotnet.microsoft.com)
- Entity Framework Core
- JWT Bearer (`Microsoft.AspNetCore.Authentication.JwtBearer`)
- Two-Factor Auth via [Otp.NET](https://github.com/kspearrin/Otp.NET)
- Custom exception middleware
- User model and migration generated

---

## 🚀 How to Run

1. **Clone the repository**
   ```bash
   git clone https://github.com/your-user/your-repo.git
   cd your-repo

2. **Set appsettings.json**
   ```bash
   {
     "ConnectionStrings": {
     "DefaultConnection": "Username=;Password=;Host=;Port=5432;Database=;",
     "RedisConnection": "localhost:6379,password="
     },
     "Jwt": {
       "Secret": "PROVIDE YOUR SECRET TOKEN HERE",
       "Issuer": "Issuer",
       "Audience": "Audience",
       "ExpiresIn": "24"
     }
   }

3. **Run project**
   ```bash
   dotnet restore
   dotnet run
