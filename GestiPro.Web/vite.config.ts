import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      // Redireciona chamadas /api para o GestiPro.API
      '/api': 'http://localhost:5059',
    },
  },
})
