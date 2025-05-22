# WebApiChallenge

## Descrição do Projeto

O **WebApiChallenge** é uma API RESTful desenvolvida em .NET 8, voltada para gestão de entidades como Dentistas, Usuários, Previsões e integrações com IA generativa. O projeto adota práticas modernas de Clean Code e possui uma suíte abrangente de testes automatizados, garantindo alta qualidade, manutenibilidade e confiabilidade do código.

---

## Integrantes do Grupo

- **Caio Eduardo Nascimento Martins - RM554025**
- **Julia Mariano Barsotti Ferreira - RM552713**
- **Leonardo Gaspar Saheb - RM553383**

## Funcionalidades

- CRUD completo para Dentistas, Usuários, Previsões e outros recursos
- Integração com modelos de IA generativa via ML.NET (ex: análise de sentimento)
- Integração com serviços externos (ex: Stripe para pagamentos)
- Testes unitários, de integração e de sistema implementados com xUnit e Moq
- Logging estruturado e tratamento de erros
- Práticas de Clean Code aplicadas em toda a base

---

## Práticas de Clean Code Aplicadas

- **Responsabilidade única:** Controllers e serviços seguem o princípio SRP, cada classe tem uma única responsabilidade.
- **Injeção de dependências:** Uso extensivo de interfaces e injeção de dependências para facilitar testes e manutenção.
- **Nomenclatura clara:** Métodos, variáveis e classes possuem nomes descritivos e objetivos.
- **Validação explícita:** Checagem de dados de entrada e tratamento de erros consistente.
- **Separação de camadas:** DTOs, Models, Repositories e Controllers bem definidos.
- **Código limpo e comentado:** Código enxuto, sem duplicidade e com comentários apenas quando necessário.

---

## Testes Automatizados

### Estratégia de Testes

O projeto possui cobertura de testes para os principais cenários:

- **Testes Unitários:**
Cobrem controllers e serviços isoladamente, utilizando mocks para dependências.
Exemplo de frameworks: xUnit, Moq.

### Exemplos de Casos de Teste

- **Controllers:**
    - Retorno correto para buscas, criações, atualizações e deleções
    - Validação de dados obrigatórios e tratamento de erros
    - Verificação de chamadas a métodos dos repositórios
- **IA Generativa:**
    - Testes de endpoints que utilizam modelos ML.NET para análise de sentimento, validando previsões e respostas do modelo
- **Pagamentos:**
    - Integração e simulação de chamadas ao Stripe, validando respostas e tratamento de exceções
      
### Como Executar os Testes

```bash
dotnet test
```

Todos os testes estão localizados na pasta `/test` e seguem a estrutura de organização.

---

## Funcionalidades de IA Generativa

- **Análise de Sentimento:**
Integração com ML.NET para análise de sentimento em textos enviados via API, utilizando modelos pré-treinados para facilitar a adoção e evitar complexidade de treinamento manual.

---

## Como Rodar a Aplicação

1. Clone o repositório:

```bash
git clone https://github.com/caioedum/challenge-dotnet-sprint4.git
```

2. Instale as dependências:

```bash
dotnet restore
```

3. Configure variáveis de ambiente e chaves de API necessárias (ex: Stripe, ML.NET).
4. Execute a aplicação:

```bash
dotnet run --project src/WebApiChallenge
```

---

## Como Rodar os Testes

```bash
dotnet test
```

Todos os testes podem ser executados diretamente pelo comando acima. O resultado exibirá a cobertura e os cenários validados.

---

## Acesse a API no navegador ou via Postman:

- **🔗 Endpoint padrão:**
```
https://localhost:7185
```
- **📜 Documentação Swagger:**
```
https://localhost:7185/swagger/index.html
```

## Endpoints da API

🔹 Endpoints:

### AtendimentosUsuarios

- GET /api/AtendimentosUsuarios - Retorna todos os atendimentos

- POST /api/AtendimentosUsuarios - Adiciona um novo atendimento
  
- GET /api/AtendimentosUsuarios/{id} - Retorna por Id

- PUT /api/AtendimentosUsuarios/{id} - Atualiza um atendimento

- DELETE /api/AtendimentosUsuarios/{id} - Remove um atendimento

### ContatosUsuarios

- GET /api/ContatosUsuarios - Retorna todos os contatos

- POST /api/ContatosUsuarios - Adiciona um novo contato
  
- GET /api/ContatosUsuarios/{id} - Retorna por Id

- PUT /api/ContatosUsuarios/{id} - Atualiza um contato

- DELETE /api/ContatosUsuarios/{id} - Remove um contato

### Dentistas

- GET /api/Dentistas - Retorna todos os dentistas

- POST /api/Dentistas - Adiciona um novo dentista
  
- GET /api/Dentistas/{id} - Retorna por Id

- PUT /api/Dentistas/{id} - Atualiza um dentista

### Previsoes

- GET /api/Previsoes - Retorna todas as previsões

- POST /api/Previsoes - Adiciona uma nova previsão
  
- GET /api/Dentistas/{id} - Retorna por Id

- PUT /api/Dentistas/{id} - Atualiza uma previsão

- DELETE /api/Dentistas/{id} - Remove uma nova previsão

### Usuarios

- GET /api/Usuarios - Retorna todos os usuários

- POST /api/Usuarios - Adiciona um novo usuário
  
- GET /api/Usuarios/{id} - Retorna por Id

- PUT /api/Usuarios/{id} - Atualiza um usuário

## 📜 Licença

- 📝 Este projeto é de uso acadêmico - FIAP.

