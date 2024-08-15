/// <reference types="cypress" />

describe('login', () => {
  it('forms authentication', () => {
    // cy.login('takerman', 'Hakerman91!');
    cy.visit('/login');
    cy.get('input[id=email]').type('tanyo@takerman.net');
    cy.get('input[id=password]').type('Hakerman91!');
    cy.get('button[id=btnSubmit]').click();
    cy.url().should('include', '/');
  });

  it("facebook authentication", () => {});
  it("google authentication", () => {});
});
