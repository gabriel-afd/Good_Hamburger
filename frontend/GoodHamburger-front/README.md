# GoodHamburger Front

Frontend da aplicação GoodHamburger desenvolvido com Angular 19. A interface concentra o fluxo de pedidos, permitindo listar, criar, editar e remover pedidos com cálculo de combos e descontos.

## Stack utilizada

- Angular 19
- Angular Material
- RxJS
- Signals
- Reactive Forms

## Bibliotecas principais

As dependências centrais do projeto incluem:

- `@angular/core`, `@angular/router`, `@angular/forms`
- `@angular/material` e `@angular/cdk`
- `rxjs`
- `@angular/ssr`
- `express`

## Como rodar o projeto

### Pré-requisitos

- Node.js instalado
- npm instalado

### Instalação

```bash
npm install
```

### Ambiente de desenvolvimento

```bash
npm start
```

O projeto sobe localmente em `http://localhost:4200`.

### Build

```bash
npm run build
```

## Estrutura do projeto

```text
src/
	app/
		core/
			services/
				menu-item.service.ts
				order.service.ts
		models/
			menu-item.model.ts
			order-item.model.ts
			order.model.ts
		pages/
			orders/
				orders.component.ts
				orders.component.html
				orders.component.scss
		shared/
			components/
				order-dialog/
					order-dialog.component.ts
					order-dialog.component.html
					order-dialog.component.scss
		app.component.ts
		app.config.ts
		app.routes.server.ts
	index.html
	main.ts
	main.server.ts
	server.ts
	styles.scss
public/
angular.json
package.json
```

## Organização da aplicação

- `core/services`: serviços responsáveis pela comunicação e regras de acesso aos dados de menu e pedidos.
- `models`: contratos e tipagens da aplicação.
- `pages/orders`: página principal de gerenciamento de pedidos.
- `shared/components/order-dialog`: modal reutilizável para criação e edição de pedidos.
- `styles.scss`: estilos globais e customizações visuais.

## Como as libs são usadas no projeto

### Angular Material

Usado na composição da interface com componentes como diálogo, botões, cards, chips, ícones, snackbars, tooltips e spinner de carregamento.

### RxJS

Usado principalmente nos fluxos assíncronos dos serviços e nas subscriptions para carregamento, criação, edição e remoção de pedidos.

### Signals

Usado para estado local dos componentes, incluindo listas, loading, itens selecionados e valores derivados como subtotal, desconto e total.

### Reactive Forms

Usado no modal de pedidos para montar e controlar o formulário de criação e edição de pedidos.

## Scripts disponíveis

- `npm start`: inicia o servidor de desenvolvimento.
- `npm run build`: gera a build da aplicação.
- `npm run watch`: gera build em modo watch.

