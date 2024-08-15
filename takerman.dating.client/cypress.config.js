const { defineConfig } = require("cypress");

module.exports = defineConfig({
  e2e: {
    baseUrl: 'https://localhost:5175', // 'https://sreshti.net'
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
