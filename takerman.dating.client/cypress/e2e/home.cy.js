/// <reference types="cypress" />

describe('home', () => {
  it('successfully loads', () => {
    cy.visit('/');
    cy.url().should('include', '/');
  });

  it("has elements on the page", () => {
    cy.visit('/');
    cy.get('#filter').should('exist');
    cy.get('#languageSelector').should('exist');
    cy.get('#watchlist').should('exist');
    cy.get('#datesList').should('exist');
    cy.get('footer').should('exist');
    cy.get('#headerNavigation').should('exist');
    cy.get('#newsletterForm').should('exist');
    cy.get('#socialBar').should('exist');
    cy.get('#zsiq_float').should('exist');
  });

  it("navigation", () => {
    // visit a date
    // privacy
    // terms and conditions
    // contact us
    // social links

    // if not logged in:
    // register
    // login

    // if logged in:
    // my dates
    // matches
    // profile
    // photos

    // if logged in as admin:
    // admin
  });
});
