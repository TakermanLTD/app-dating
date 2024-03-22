/// <reference types="cypress" />

describe('home', () => {
  it('successfully loads', () => {
    cy.visit('/');
    cy.url().should('include', '/');
  })
})
