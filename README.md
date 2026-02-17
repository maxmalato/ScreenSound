# 🎵 ScreenSound

![Badge em Desenvolvimento](http://img.shields.io/static/v1?label=STATUS&message=CONCLUÍDO&color=GREEN&style=for-the-badge)
![Badge .NET](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![Badge Blazor](https://img.shields.io/badge/Blazor-512BD4?style=for-the-badge&logo=blazor&logoColor=white)
![Badge Azure SQL](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)

> Uma aplicação Full Stack moderna para gestão de catálogos musicais, avaliação de artistas e classificação de gêneros.

---

## 🖼️ Visualização do Projeto

O ScreenSound oferece uma experiência visual rica e responsiva, permitindo gerenciar músicas e artistas de forma intuitiva.

### ⚙️ API
Visão geral dos endpoints da API do projeto.

<img width="2880" height="7386" alt="api" src="https://github.com/user-attachments/assets/079ed711-ff2e-49e2-aa88-6fb3125a4ce0" />


### 🏠 Dashboard e Home
Uma visão geral com carrossel de destaques e estatísticas do sistema.

<img width="2880" height="2172" alt="login" src="https://github.com/user-attachments/assets/5447c67e-315a-43e5-96f1-a9f0a9d795f4" />


### 🎵 Artistas, Gêneros e Músicas
Exibição das páginas dos artistas, músicas e gêneros.

<img width="2880" height="2582" alt="artists_view" src="https://github.com/user-attachments/assets/d8340a49-6544-4e7e-99a2-26e9662768cf" />

--

<img width="2880" height="1342" alt="musics_view" src="https://github.com/user-attachments/assets/8d434f74-92a3-4170-9684-e13c97edb68a" />

--

<img width="2880" height="1342" alt="genrer_view" src="https://github.com/user-attachments/assets/f0d07e9b-4cf9-4104-ba88-dc466df333e7" />


---

## 🚀 Sobre o Projeto

O **ScreenSound** é um sistema web desenvolvido para consolidar conhecimentos em desenvolvimento Full Stack com .NET. A aplicação permite o cadastro, edição e exclusão de artistas, músicas e gêneros, além de oferecer um sistema de avaliação e busca inteligente.

O projeto foi estruturado utilizando **Clean Architecture** e princípios **SOLID**, separando as responsabilidades entre a API (Backend) e a Interface Web (Frontend).

---

## ⚡ Funcionalidades Principais

- **Gestão de Artistas:** CRUD completo com foto de perfil e biografia.
- **Catálogo Musical:** Associação de músicas a artistas e gêneros múltiplos.
- **Sistema de Avaliação:** Usuários autenticados podem avaliar artistas (rating de 1 a 5 estrelas).
- **Dashboard Interativo:** Carrossel com artistas em destaque e contadores de registros.
- **Busca Híbrida:** Pesquisa inteligente que filtra tanto por nome da música quanto do artista.
- **Autenticação e Segurança:** Login, Registro e proteção de rotas administrativas.
- **Design Moderno:** Interface construída com **MudBlazor** (Material Design), suportando Tema Escuro (Dark Mode).

---

## 🛠️ Tecnologias Utilizadas

### Backend (API)
- **C# .NET 8**
- **Entity Framework Core** (ORM)
- **SQL Server** (Banco de Dados)
- **Identity** (Autenticação)

### Frontend (Web)
- **Blazor WebAssembly**
- **MudBlazor** (Component Library)
- **HTML5 / CSS3**

---

## 📚 Origem e Evolução

Este projeto foi iniciado durante a formação de .NET da **[Alura](https://www.alura.com.br/)**. 

A partir da base ensinada no curso, realizei diversas **implementações e melhorias próprias**, incluindo:
1.  **Refatoração da UI:** Migração completa para **MudBlazor**, criando um visual profissional e responsivo.
2.  **Dashboard:** Criação de uma tela inicial com métricas e carrossel.
3.  **Regras de Negócio:** Implementação de proteção para não excluir artistas que possuem músicas vinculadas.
4.  **Otimização de Performance:** Melhorias nas consultas ao banco de dados e filtros de pesquisa.
5.  **UX/UI:** Ajustes finos de usabilidade, feedbacks visuais (Snackbars) e tratamento de erros amigável.

---

## 📦 Como Rodar o Projeto Localmente

### Pré-requisitos
- .NET 8 SDK instalado.
- SQL Server (LocalDB ou Container Docker).
- Visual Studio 2022 ou Rider.

### Passo a Passo

1. **Clone o repositório**
   ```bash
   git clone [https://github.com/SEU-USUARIO/ScreenSound.git](https://github.com/SEU-USUARIO/ScreenSound.git)

2. **Configure o Banco de Dados**
   ```bash
   cd ScreenSound.API
   dotnet ef database update

3. **Execute a API**
- Inicie o projeto ScreenSound.API.

4. **Execute o Frontend**
- Inicie o projeto ScreenSound.Web.
- O navegador abrirá a aplicação pronta para uso!

## 👨‍💻 Autor
**Desenvolvido por:** ***Maxjannyfer Malato***.
