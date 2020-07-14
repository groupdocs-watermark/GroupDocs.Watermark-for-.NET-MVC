import { getGreeting } from '../support/app.po';

describe('watermark', () => {
  beforeEach(() => cy.visit('/'));

  it('should display welcome message', () => {
    getGreeting().contains('Welcome to watermark!');
  });
});
