/// <reference types="cypress" />

describe('scenarios', () => {
  it("navigation", () => {
    cy.visit('/logout');

    cy.get('#headerAccount').click();
    cy.get('#headerRegister').click();
    cy.location('pathname').should('eq', '/register');
    cy.go('back');

    cy.visit('/logout');
    cy.visit('/login');
    cy.get('input[id=email]').type('tanyo@takerman.net');
    cy.get('input[id=password]').type('Hakerman91!');
    cy.get('button[id=btnSubmit]').click();

    cy.get('#headerOrders').click();
    cy.location('pathname').should('eq', '/orders');
    cy.go('back');

    cy.get('#headerMatches').click();
    cy.location('pathname').should('eq', '/matches');
    cy.go('back');

    cy.get('#headerAdmin').click();
    cy.location('pathname').should('eq', '/admin');
    cy.go('back');

    cy.get('#headerAccount').click();
    cy.get('#headerProfile').click();
    cy.location('pathname').should('eq', '/profile');
    cy.go('back');

    cy.get('#headerAccount').click();
    cy.get('#headerGallery').click();
    cy.location('pathname').should('eq', '/user-gallery');
    cy.go('back');

  });

  it("register", () => { });
  it("save date", () => { });
  it("buy date", () => { });
  it("enter date", () => { });
  it("vote", () => { });
  it("chat", () => { });
  it("full scenario", () => {
    for (let i = 0; i < 10; i++) {
      // register
      cy.visit('/register');
      cy.get('input[id=firstName]').type('Test' + i);
      cy.get('input[id=lastName]').type('Test' + i);
      cy.get('input[id=email]').type('test' + i + '@takerman.net');
      if (i < 5) {
        cy.get('#genderMan').check();
      } else {
        cy.get('#genderWoman').check();
      }
      cy.get('input[id=password]').type('Hakerman91!');
      cy.get('input[id=confirmPassword]').type('Hakerman91!');
      cy.get('button[id=btnSubmit]').click();
      cy.url().should('include', '/');

      // save spot
      // logout

      // login as admin
      // approve the date
      // logout

      // for each ten customers
      // login
      // buy the date
      // logout

      // for each ten customers
      // login
      // visit the link
      // vote
      // check your matches
    }
  });
});
