const { defineConfig } = require("cypress");

module.exports = defineConfig({
  e2e: {
    baseUrl: 'https://sreshti.net', // 'https://localhost:5175'
    setupNodeEvents(on, config) {
    }
  },
  component: {
    devServer: {
      framework: 'vue',
      bundler: 'vite',
    },
  },
  projectId: "y7snqm"
});
