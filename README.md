
# 🎬 Wolflix

Wolflix é um projeto de **streaming de vídeos** inspirado na Netflix, desenvolvido com o objetivo de **estudo e aplicação prática de TDD (Test-Driven Development)**, **DDD (Domain-Driven Design)** e **Clean Code**.  

O projeto está sendo construído em **.NET 6** e, neste momento, encontra-se na **fase de implementação e testes do domínio (Domain Layer)**.

---

## 🚀 Objetivo do Projeto

O Wolflix é um projeto de estudo criado para:
- Praticar **boas práticas de design orientado ao domínio (DDD)**;
- Implementar **testes unitários utilizando xUnit e FluentAssertions**;
- Aplicar **conceitos de Clean Code, SOLID e arquitetura em camadas**;
- Estruturar uma base sólida para futuras implementações de **aplicação, infraestrutura e API**;
- Evoluir gradualmente até um sistema completo de **streaming de vídeos** com catálogo, usuários, categorias, etc.

---

## 🧩 Estrutura Atual do Projeto

```

JB.Wolflix.Catalog/
│
├── src/
│   └── JB.Wolflix.Catalog.Domain/
│       ├── Entities/
│       ├── Exceptions/
│       ├── SeedWork/
│       ├── Utils/
│       └── Validation/
│
└── tests/
└── JB.Wolflix.Catalog.UnitTests/

````

### 📦 Projeto de Domínio (`JB.Wolflix.Catalog.Domain`)
Contém as regras de negócio principais do sistema.

- **Entities** → Entidades do domínio (ex: `Category`)
- **Exceptions** → Exceções específicas do domínio (ex: `EntityValidationException`)
- **SeedWork** → Classes base e contratos compartilhados entre entidades
- **Utils** → Utilitários como `CategoryExceptionMessage`
- **Validation** → Validações de domínio, como `DomainValidation`

### 🧪 Projeto de Testes (`JB.Wolflix.Catalog.UnitTests`)
Conjunto de testes unitários com foco no domínio, utilizando:

- [x] **xUnit** — Framework de testes
- [x] **FluentAssertions** — Escrita fluente e legível nos asserts
- [x] **Bogus** — Geração de dados falsos para testes
- [x] **coverlet.collector** — Coleta de cobertura de testes

---

## ⚙️ Tecnologias Utilizadas

- **.NET 6**
- **C# 10**
- **xUnit**
- **FluentAssertions**
- **Bogus**
- **Coverlet**
- **Visual Studio / VS Code**

---

## 🔬 Abordagem de Desenvolvimento

O projeto segue a metodologia **TDD (Test-Driven Development)**:

1. **Escreva o teste** (Red)  
2. **Implemente o código mínimo necessário** (Green)  
3. **Refatore o código e o teste** (Refactor)

Essa abordagem garante um código mais **confiável, modular e testável**.

---

## 📅 Status do Projeto

🧱 **Em desenvolvimento**  
Atualmente na **fase de testes e modelagem do domínio**.  
As próximas etapas incluem:
- Criação da camada de **Application Services**
- Implementação da **Infraestrutura**
- Exposição de **APIs RESTful**
- Integração com **banco de dados relacional**
- Autenticação e controle de usuários
- Implementação do **catálogo de vídeos completo**

---

## 🧑‍💻 Como Executar os Testes

Dentro da pasta do projeto de testes:

```bash
dotnet test
````

---

## 📘 Licença

Este projeto está sob fins **educacionais** e de **estudo pessoal**.
Nenhum conteúdo audiovisual real é hospedado ou distribuído.

---

## 💡 Autor

**João Barbosa (BarbosaCode)**
📧 Contato: [em breve]
🚀 Projeto Wolflix - "Aprendendo na prática com TDD, DDD e Clean Code"


