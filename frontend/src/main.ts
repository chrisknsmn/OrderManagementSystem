import { createApp } from 'vue'
import { createRouter, createWebHistory } from 'vue-router'
import App from './App.vue'

// Import Bootstrap CSS and JS
import 'bootstrap/dist/css/bootstrap.min.css'
import 'bootstrap/dist/js/bootstrap.bundle.min.js'

// Import views
import Customers from './views/Customers.vue'
import Vehicles from './views/Vehicles.vue'
import RepairOrders from './views/RepairOrders.vue'

const routes = [
  { path: '/', redirect: '/repair-orders' },
  { path: '/customers', name: 'Customers', component: Customers },
  { path: '/vehicles', name: 'Vehicles', component: Vehicles },
  { path: '/repair-orders', name: 'RepairOrders', component: RepairOrders },
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

const app = createApp(App)
app.use(router)
app.mount('#app')