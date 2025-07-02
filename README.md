PathbitChallenge -
Essa é uma API REST maneirinha pra gerenciar usuários, clientes, produtos e pedidos. Feita com .NET 8.0, usando DDD, SOLID, Clean Code, PostgreSQL, Entity Framework Core, autenticação JWT, Swagger pra documentação e testes unitários pra garantir que tá tudo nos conformes.
O que tem na parada?

Usuários e Clientes: Cadastro, login, CRUD completo (só pra admins, exceto pedidos que são pros clientes).
Produtos: Gerenciamento de estoque (criar, listar, atualizar, deletar).
Pedidos: Criação com validação de CEP via API externa, além de CRUD.
Autenticação: JWT pra proteger os endpoints.
Testes: Cobertura de testes unitários com xUnit e Moq.
Docker: Tudo prontinho pra rodar com docker-compose.

Estrutura do Projeto
PathbitChallenge/
├── scripts/db/init.sql # Script pra criar o banco
├── src/
│ ├── Api/ # Controladores, configs da API
│ ├── Application/ # DTOs, serviços
│ ├── Common/ # Coisas compartilhadas
│ ├── Domain/ # Entidades, interfaces
│ ├── Infrastructure/ # Conexão com banco
├── tests/UnitTests/ # Testes unitários
├── docker-compose.yml # Config do Docker
├── PathbitChallenge.sln # Solução .NET
├── .gitignore # Arquivos ignorados pelo Git
└── README.md # Esse arquivo aqui

O que você precisa pra rodar?

.NET SDK 8.0: Pra compilar e rodar.
Docker Desktop: Pra rodar com contêineres (banco e API).
Git: Pra clonar o projeto.
PostgreSQL (opcional): Se quiser rodar o banco localmente.
Postman ou Swagger: Pra testar os endpoints.

Como botar pra rodar localmente?

Clona o repositório:
cd "C:\Users\felipe kim\OneDrive\Área de Trabalho\fbhk"
git clone https://github.com/seu-usuario/PathbitChallenge.git
cd PathbitChallenge

Instala as dependências:
dotnet restore

Configura o banco (se não usar Docker):

Edite src\Api\appsettings.json com a connection string do seu PostgreSQL:"ConnectionStrings": {
"DefaultConnection": "Host=localhost;Database=pathbit_challenge;Username=seu-usuario;Password=sua-senha"
}

Rode o script do banco:psql -U seu-usuario -d postgres -f scripts\db\init.sql

Compila e roda a API:
dotnet build
cd src\Api
dotnet run

Testa a API:

Abre http://localhost:5000/swagger no navegador pra usar o Swagger.
Ou usa o Postman pra mandar requisições.

Rodando com Docker (mais fácil ainda!)

Sobe os contêineres:
cd "C:\Users\felipe kim\OneDrive\Área de Trabalho\fbhk\PathbitChallenge"
docker-compose up --build

Acessa a API:

Vai em http://localhost:5000/swagger pra testar.

Para tudo:
docker-compose down

Endpoints da API
Autenticação

POST /api/auth/signup: Cadastra um cliente e usuário.
Exemplo:{
"name": "João da Silva",
"email": "joao@example.com",
"password": "senha123"
}

Retorna: { "id": "guid", "email": "joao@example.com", "username": "joao@example.com", "name": "João da Silva" }

POST /api/auth/login: Pega o token JWT.
Exemplo:{
"username": "joao@example.com",
"password": "senha123"
}

Retorna: { "token": "jwt-token" }

Usuários (só pra admins)

POST /api/users: Cria usuário ({ "email": "user@example.com", "username": "user", "password": "senha123", "userType": "CLIENTE" }).
GET /api/users/{id}: Busca usuário por ID.
GET /api/users: Lista todos os usuários.
PUT /api/users/{id}: Atualiza usuário ({ "email": "novo@example.com", "username": "novo", "userType": "ADMINISTRADOR" }).
DELETE /api/users/{id}: Deleta usuário.

Clientes (só pra admins)

POST /api/customers: Cria cliente ({ "name": "Maria", "email": "maria@example.com" }).
GET /api/customers/{id}: Busca cliente por ID.
GET /api/customers: Lista todos os clientes.
PUT /api/customers/{id}: Atualiza cliente ({ "name": "Maria Silva", "email": "maria.silva@example.com" }).
DELETE /api/customers/{id}: Deleta cliente.

Produtos (só pra admins)

POST /api/products: Cria produto ({ "name": "Produto X", "price": 10.0, "quantityAvailable": 100 }).
GET /api/products/{id}: Busca produto por ID.
GET /api/products: Lista todos os produtos.
PUT /api/products/{id}: Atualiza produto ({ "name": "Produto Y", "price": 20.0, "quantityAvailable": 200 }).
DELETE /api/products/{id}: Deleta produto.

Pedidos (só pra clientes)

POST /api/orders: Cria pedido ({ "productId": "guid", "quantity": 10, "deliveryCep": "12345678" }).
GET /api/orders/{id}: Busca pedido por ID.
GET /api/orders: Lista todos os pedidos.
PUT /api/orders/{id}: Atualiza pedido ({ "quantity": 15, "deliveryCep": "87654321" }).
DELETE /api/orders/{id}: Deleta pedido.

Autenticação

Pegue o token no /api/auth/login.
Adicione no header Authorization: Bearer <token> pra acessar endpoints protegidos.

Testando a Cobertura

Roda os testes:
cd tests\UnitTests
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

Gera o relatório de cobertura:Instala o ReportGenerator (se não tiver):
dotnet tool install -g dotnet-reportgenerator-globaltool

Gera o relatório:
reportgenerator -reports:coverage.opencover.xml -targetdir:coverage-report -reporttypes:Html

Abre o coverage-report/index.html no navegador pra ver o resultado.

Se der ruim (ex.: Erro CS0246 - ProductDTO não encontrado)
Se aparecer o erro CS0246: O tipo ou nome de namespace 'ProductDTO' não pôde ser encontrado:

Checa o DTOs.cs:
Confirma que src\Application\DTOs\DTOs.cs tem o ProductDTO:public record ProductDTO(Guid Id, string Name, decimal Price, int QuantityAvailable);

Adiciona o using:
Nos arquivos que usam ProductDTO (como ProductsController.cs, ProductService.cs, ProductServiceTests.cs), adiciona:using PathbitChallenge.Application.DTOs;

Confirma referências:
Garante que os projetos PathbitChallenge.Api e PathbitChallenge.UnitTests referenciam PathbitChallenge.Application:cd src\Api
dotnet add reference ..\Application\PathbitChallenge.Application.csproj
cd ..\..\tests\UnitTests
dotnet add reference ..\..\src\Application\PathbitChallenge.Application.csproj
cd ..\..

Compila de novo:dotnet build

Commit no Git
Depois de aplicar as mudanças:
cd "C:\Users\felipe kim\OneDrive\Área de Trabalho\fbhk\PathbitChallenge"
git add .
git commit -m "Atualizado README com vibe mais de boa e corrigido erro CS0246"
git push origin main

Fale comigo!
Se tiver dúvidas ou quiser melhorar algo, manda um e-mail pra [seu-email@example.com] ou abre uma issue no GitHub. Bora fazer essa API ficar ainda mais irada!# PathbitChallenge
