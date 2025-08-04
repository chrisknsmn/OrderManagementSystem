import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

export default defineConfig({
  plugins: [vue()],
  build: {
    outDir: '../wwwroot',
    emptyOutDir: true,
    rollupOptions: {
      output: {
        manualChunks: undefined,
      },
    },
  },
  server: {
    proxy: {
      '/api': {
        target: 'http://localhost:5027',
        changeOrigin: true,
        secure: false,
      },
    },
  },
})