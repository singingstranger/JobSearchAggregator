
import react from '@vitejs/plugin-react'
import { defineConfig } from 'vite'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  base: '/CodingPortfolio/JJA-JobSearch/',
  
  server:{
    proxy: {
      '/api': {
        target: 'http://localhost:5086',
        changeOrigin: true,
        secure: false
      }
    }
  }
})
