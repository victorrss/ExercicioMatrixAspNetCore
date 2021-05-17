# Programa de Aceleração Matrix
**Proposta de exercício:**

Criar API em ASP NET Core com as seguintes funcionalidades:

1) Rota de geração de token/autenticação (Login)
Descrição: Criar uma rota na API que recebe um usuário e senha e retorna um token JWT com as permissões desse usuário;
a) Lista de usuários inicial pode estar hardcoded;
b) Definir algumas permissões (Roles) que o sistema exigirá em outras rotas de acesso;

2) Rotas de CRUD gerencialmento de usuários;
- Rota para consulta de usuários e suas permissões, ou seja, usuários existentes;
- Rota para criação de novos usuários já com suas eventuais permissões;
- Rota para edição de usuários e suas permissões;
- Rota para remoção de usuários e consequentemente suas permissões;

**Observação:** Garantir que o usuário possui permissão para acionamento da rota (a partir do token);

**Informações gerais:** O sistema pode começar com uma lista estática de usuários para realização de login e obtenção do token. As rotas de CRUD podem manipular essa lista simulando uma tabela de banco de dados.

**Diferenciais:**
1) Utilizar DI para injetar dependências, por exemplo, o service que realiza as ações (ex: UsuarioService);
2) Objetos recebidos são ViewModels e devem ser convertidos pra objetos de domínio;
3) AutoMapper para resolver conversão de DTO (Data-transfer-objects);
4) Utilizar RoleBasedAuthentication (exigir Roles) para garantir o acesso às rotas da API;
5) Permissões específicas para cada operação do CRUD: Consultar, Editar, Inserir e Apagar;
6) Uso do pattern IRepository na camada de serviço para abstrair a fonte de dados do sistema;
7) Swagger para acionamento da API;