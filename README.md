# Backend Challenge - API de Gerenciamento de Usuários

API REST desenvolvida em C# com ASP.NET Core para gerenciamento de usuários, seguindo os requisitos propostos no desafio backend.

A aplicação permite realizar as operações de CRUD (Create, Read, Update e Delete), além de possuir validações de regras de negócio, documentação com Swagger e scripts T-SQL.

## Tecnologias utilizadas

- C#
- ASP.NET Core
- .NET
- Swagger / OpenAPI
- T-SQL
- BCrypt para hash de senha
- Injeção de Dependência

## Arquitetura

A aplicação foi organizada em camadas para separar as responsabilidades e facilitar a manutenção do código.

### Controllers

Responsáveis por receber as requisições HTTP, chamar os serviços necessários e retornar as respostas HTTP adequadas.

### Services

Responsáveis pelas regras de negócio da aplicação, como:

- validação dos dados do usuário;
- validação de CPF;
- normalização do CPF;
- verificação de CPF duplicado;
- verificação de e-mail duplicado;
- geração do hash da senha;
- tratamento das regras de criação, atualização e exclusão.

### Repositories

Responsáveis pelo acesso e armazenamento dos dados.

Para este desafio foi utilizada uma lista em memória, conforme permitido pelos requisitos.

### Interfaces

Foi utilizada uma interface para abstrair o repositório, reduzindo o acoplamento entre o Service e a implementação responsável pelo armazenamento dos dados.

### DTOs

Os DTOs são utilizados para controlar os dados recebidos e retornados pela API.

Dessa forma, informações internas e sensíveis, como `PasswordHash`, não são retornadas ao cliente.

## Estrutura do projeto

```text
BackendChallange.Api/
│
├── Controllers/
│   └── UsersController.cs
│
├── Database/
│   ├── 01_CreateTable.sql
│   ├── 02_InsertUser.sql
│   ├── 03_SelectUsers.sql
│   ├── 04_SelectUserById.sql
│   ├── 05_UpdateUser.sql
│   └── 06_DeleteUser.sql
│
├── DTOs/
│   ├── CreateUserRequest.cs
│   ├── UpdateUserRequest.cs
│   └── UserResponse.cs
│
├── Interfaces/
│   └── IUserRepository.cs
│
├── Models/
│   └── User.cs
│
├── Repositories/
│   └── UserRepository.cs
│
├── Services/
│   └── UserService.cs
│
└── Program.cs
```

## Funcionalidades

A API possui as seguintes funcionalidades:

| Método | Endpoint | Descrição |
|---|---|---|
| GET | `/api/Users` | Lista todos os usuários |
| GET | `/api/Users/{id}` | Busca um usuário pelo ID |
| POST | `/api/Users` | Cadastra um novo usuário |
| PUT | `/api/Users/{id}` | Atualiza um usuário |
| DELETE | `/api/Users/{id}` | Exclui um usuário |

## Cadastro de usuário

Para cadastrar um usuário são necessários:

```json
{
  "name": "Nome",
  "email": "Ex@email.com",
  "password": "123456",
  "cpf": "000.000.000-000",
  "birthDate": "2000-01-01"
}
```

O ID é gerado automaticamente pela aplicação.

A senha recebida não é armazenada diretamente. Antes de ser salva, ela é transformada em hash.

O `PasswordHash` também não é retornado nas respostas da API.

## Validação de CPF

A aplicação aceita CPF com ou sem formatação.

Exemplo:

```text
097.120.693-70
```

O CPF é normalizado internamente:

```text
09712069370
```

Depois da normalização é realizada a validação dos dígitos verificadores do CPF.

CPFs inválidos não são aceitos pela aplicação.

## Regras de negócio

A API possui regras para impedir:

- cadastro com CPF inválido;
- cadastro de usuários com o mesmo CPF;
- cadastro de usuários com o mesmo e-mail;
- atualização utilizando CPF pertencente a outro usuário;
- atualização utilizando e-mail pertencente a outro usuário;
- operações sobre usuários inexistentes.

As regras de negócio ficam concentradas principalmente na camada `Service`.

## Segurança da senha

A senha do usuário não é armazenada em texto puro.

A aplicação gera um hash da senha antes de armazená-la.

Além disso, a API utiliza um DTO de resposta (`UserResponse`) para impedir que o hash da senha seja exposto ao cliente.

## Status HTTP

A API utiliza códigos HTTP adequados para representar o resultado das operações, incluindo:

- `200 OK`
- `201 Created`
- `400 Bad Request`
- `404 Not Found`
- `409 Conflict`

Os possíveis retornos dos endpoints também estão documentados no Swagger.

## Swagger

A API possui documentação utilizando Swagger/OpenAPI.

Com a aplicação em execução, a documentação pode ser acessada pelo endpoint:

```text
/swagger
```

No Swagger é possível visualizar e testar os endpoints disponíveis na API.

## Queries T-SQL

A pasta `Database` contém queries T-SQL correspondentes às operações da API:

- criação da tabela `Users`;
- inserção de usuário;
- listagem de usuários;
- busca de usuário por ID;
- atualização de usuário;
- exclusão de usuário.

Embora a aplicação utilize armazenamento em memória, os scripts representam como as operações seriam realizadas em um banco de dados relacional.

## Executando o projeto

Clone o repositório:

```bash
git clone https://github.com/IranSousa18/BackendChallenge.git
```

Entre na pasta da API:

```bash
cd BackendChallange.Api
```

Restaure as dependências:

```bash
dotnet restore
```

Execute a aplicação:

```bash
dotnet run
```

Após iniciar a aplicação, acesse o Swagger utilizando a URL exibida no terminal seguida de:

```text
/swagger
```

## Considerações

O projeto foi desenvolvido buscando manter uma separação clara de responsabilidades.

Apesar de o desafio exigir no mínimo as camadas Controller e Repository, foi adicionada uma camada Service para concentrar as regras de negócio, além de interfaces, DTOs e injeção de dependência.

Essa organização permite que cada parte da aplicação tenha uma responsabilidade específica e facilita futuras alterações e manutenção do projeto.