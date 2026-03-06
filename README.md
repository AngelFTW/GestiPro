# GestiPro

Sistema de controle de gastos residenciais com cadastro de pessoas, categorias, transações e consulta de totais financeiros.

---

## Tecnologias

```
Backend   -> .NET 10 / Entity Framework Core
Banco     -> SQLite
Frontend  -> React + TypeScript (Vite)
```
---

## Estrutura

```
GestiPro.API/     -> Web API REST
GestiPro.Tests/   -> Testes unitários (xUnit)
GestiPro.Web/     -> Frontend React
```

---

## Como rodar

### 1. Backend

```bash
cd GestiPro.API
dotnet run
```


### 2. Frontend

Abra um novo terminal
```bash
cd GestiPro.Web
npm install
npm run dev
```

O Vite redireciona chamadas /api automaticamente para o backend sem precisar configurar o CORS.

### 3. Testes

```bash
cd GestiPro.Tests
dotnet test
```

---

## Funcionalidades

- **Pessoas** -> cadastro completo (criar, editar, deletar, listar). Ao deletar uma pessoa, todas as suas transações são removidas automaticamente.
- **Categorias** -> criação e listagem. Cada categoria tem uma finalidade: Despesa, Receita ou Ambas.
- **Transações** -> criação e listagem, com as regras:
  - Menores de 18 anos só podem ter transações do tipo Despesa.
  - A categoria deve ser compatível com o tipo da transação.
- **Totais** -> consulta de receitas, despesas e saldo por pessoa e por categoria, com totais gerais consolidados.


## Comentários

Bom, foi um projetinho muito massa de se fazer, meu react estava um pouco enferrujado pois tenho mais vivencia com angular, porém foi ótimo pra relembrar muita coisa. O banco eu tinha pensado primeiramente em Postgres, porém é bem ruim de configurar principalmente para o objetivo desse projeto que é algo pequeno, então usei SQLite mesmo pra facilitar nossa vida. Aproveitei que to usando bastante XUnit ultimamente e coloquei no projeto também para fazer essa validações unitarias. Em relação ao front end, peguei algumas ideias de projetos antigos/cursos que possuo e na propria internet mesmo pra mudar entre White/Dark Mode e a identidade visual também. Eu tinha a ideia de subir no docker a aplicação, pra rodar só no docker compose. Mas enfim, qualquer dúvida basta entrar em contato comigo, estou sempre a disposição em qualquer horario praticamente.
