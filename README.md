# CrudAlunos API

[![.NET](https://github.com/VictorDePaula06/CrudAlunos/actions/workflows/dotnet.yml/badge.svg)](https://github.com/VictorDePaula06/CrudAlunos/actions/workflows/dotnet.yml)
Este projeto é uma API RESTful desenvolvida em **ASP.NET Core** para o gerenciamento de contatos (simulando um CRUD de alunos). Ele oferece funcionalidades para criar, listar (ativos/inativos), visualizar detalhes, ativar, inativar, editar e deletar registros de contatos.

## 🚀 Tecnologias Utilizadas

* **C#**
* **ASP.NET Core 5.0**
* **Entity Framework Core** (para ORM e acesso a dados)
* **SQL Server** (como banco de dados)
* **xUnit** (para testes unitários)
* **Swagger/OpenAPI** (para documentação interativa da API)

## ✨ Funcionalidades Principais

* **Cadastro de Contatos:** Criação de novos contatos com validação de formato de data de nascimento e idade mínima (maiores de 18 anos).
* **Listagem de Contatos:**
    * Listar todos os contatos ativos.
    * Listar todos os contatos inativos.
* **Detalhes do Contato:** Obter informações detalhadas de um contato específico por ID.
* **Ativação/Inativação:** Marcar contatos como ativos ou inativos.
* **Edição de Contatos:** Atualizar informações de contatos existentes.
* **Exclusão de Contatos:** Remover contatos do sistema.
* **Testes Unitários:** Cobertura de testes abrangente para as principais funcionalidades dos controladores.

## 🛠 Pré-requisitos

Antes de iniciar, certifique-se de ter os seguintes softwares instalados:

* [.NET SDK 5.0](https://dotnet.microsoft.com/download/dotnet/5.0) ou superior
* [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (ou Docker com SQL Server)
* Uma IDE como [Visual Studio](https://visualstudio.microsoft.com/vs/) ou [VS Code](https://code.visualstudio.com/)

## ⚙️ Configuração e Execução

Siga os passos abaixo para configurar e rodar o projeto em sua máquina local:

1.  **Clone o repositório:**
    ```bash
    git clone [https://github.com/VictorDePaula06/CrudAlunos.git](https://github.com/VictorDePaula06/CrudAlunos.git)
    cd CrudAlunos
    ```

2.  **Configurar o Banco de Dados:**
    * Abra o arquivo `Startup.cs` na pasta raiz do projeto.
    * Localize a linha de conexão com o banco de dados:
        ```csharp
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer("Server=localhost;Database=MeusContatos;Trusted_Connection=True;TrustServerCertificate=True"));
        ```
    * Certifique-se de que o `Server=localhost` (ou o nome do seu servidor SQL) e `Database=MeusContatos` estejam configurados corretamente para sua instância de SQL Server.
    * **Executar Migrações:** Para criar o banco de dados e as tabelas, navegue até a pasta do projeto `CrudAlunos` no terminal e execute os comandos do Entity Framework Core:
        ```bash
        dotnet ef database update
        ```
        *(Certifique-se de ter a ferramenta `dotnet-ef` instalada globalmente: `dotnet tool install --global dotnet-ef`)*

3.  **Rodar a Aplicação:**
    * Na pasta raiz do projeto, execute o seguinte comando:
        ```bash
        dotnet run
        ```
    * A API estará disponível em `https://localhost:5001` (HTTPS) e `http://localhost:5000` (HTTP) por padrão, ou a porta que o Visual Studio/VS Code configurar para você.

## 📄 Documentação da API (Swagger)

Após rodar a aplicação, você pode acessar a documentação interativa da API via Swagger no seu navegador. Isso permite testar os endpoints diretamente:

* Abra seu navegador e acesse: `https://localhost:5001` ou `http://localhost:5000` (ou a porta que sua aplicação estiver rodando).
    * A rota padrão do Swagger foi configurada para ser a raiz do projeto.

## 🧪 Executando os Testes

Este projeto inclui testes unitários desenvolvidos com xUnit. Para executá-los:

1.  Navegue até a pasta do projeto no terminal.
2.  Execute o comando:
    ```bash
    dotnet test
    ```

## 🤝 Contribuição

Contribuições são bem-vindas! Sinta-se à vontade para abrir issues ou pull requests.

## ✉️ Contato

* **João Victor de Paula**
* www.linkedin.com/in/joaovictor061
* J.17jvictor@gmail.com

---
