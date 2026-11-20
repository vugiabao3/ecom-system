import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],

  server: {
    allowedHosts: [
      'recreation-arranged-duck-encryption.trycloudflare.com'
    ],

    proxy: {
      '/auth': {
        target: 'http://localhost:5001',
        changeOrigin: true,
        secure: false
      },

      '/user': {
        target: 'http://localhost:5004',
        changeOrigin: true,
        secure: false
      },

      '/product': {
        target: 'http://localhost:5003',
        changeOrigin: true,
        secure: false
      },

      '/cart': {
        target: 'http://localhost:5002',
        changeOrigin: true,
        secure: false
      },

      '/order': {
        target: 'http://localhost:5290',
        changeOrigin: true,
        secure: false
      },

      '/payment': {
        target: 'http://localhost:5289',
        changeOrigin: true,
        secure: false
      },

      '/promotion': {
        target: 'http://localhost:5278',
        changeOrigin: true,
        secure: false
      },

      '/shipping': {
        target: 'http://localhost:5243',
        changeOrigin: true,
        secure: false
      },

      '/inventory': {
        target: 'http://localhost:5270',
        changeOrigin: true,
        secure: false
      }
    }
  }
})