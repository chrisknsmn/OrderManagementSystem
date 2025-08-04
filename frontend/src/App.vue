<template>
  <div id="app">
    <!-- Navigation -->
    <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
      <div class="container">
        <router-link class="navbar-brand" to="/repair-orders">
          <i class="bi bi-wrench"></i> Repair Order Management
        </router-link>
        
        <button 
          class="navbar-toggler" 
          type="button" 
          data-bs-toggle="collapse" 
          data-bs-target="#navbarNav"
          aria-controls="navbarNav" 
          aria-expanded="false" 
          aria-label="Toggle navigation"
        >
          <span class="navbar-toggler-icon"></span>
        </button>
        
        <div class="collapse navbar-collapse" id="navbarNav">
          <ul class="navbar-nav me-auto">
            <li class="nav-item">
              <router-link class="nav-link" to="/customers" :class="{ active: $route.name === 'Customers' }">
                <i class="bi bi-people"></i> Customers
              </router-link>
            </li>
            <li class="nav-item">
              <router-link class="nav-link" to="/vehicles" :class="{ active: $route.name === 'Vehicles' }">
                <i class="bi bi-car-front"></i> Vehicles
              </router-link>
            </li>
            <li class="nav-item">
              <router-link class="nav-link" to="/repair-orders" :class="{ active: $route.name === 'RepairOrders' }">
                <i class="bi bi-clipboard-check"></i> Repair Orders
              </router-link>
            </li>
          </ul>
        </div>
      </div>
    </nav>

    <!-- Main Content -->
    <main class="container mt-4">
      <router-view />
    </main>

    <!-- Footer -->
    <footer class="bg-light text-center text-muted py-3 mt-5">
      <div class="container">
        <p class="mb-0">© 2025 Repair Order Management System - Built with Vue.js, TypeScript & Bootstrap</p>
      </div>
    </footer>
  </div>
</template>

<script setup lang="ts">
// No additional setup needed for this component
</script>

<style>
/* Add Bootstrap Icons */
@import url('https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.0/font/bootstrap-icons.css');

/* Custom styles */
.navbar-brand {
  font-weight: bold;
}

.nav-link.active {
  background-color: rgba(255, 255, 255, 0.1);
  border-radius: 0.375rem;
}

main {
  min-height: calc(100vh - 200px);
}

/* Custom button styles */
.btn-outline-primary:hover {
  transform: translateY(-1px);
  transition: transform 0.2s;
}

/* Card hover effects */
.card {
  transition: box-shadow 0.3s ease;
}

.card:hover {
  box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.15);
}

/* Status badges */
.badge {
  font-size: 0.75em;
}

/* Loading spinner */
.spinner-border-sm {
  width: 1rem;
  height: 1rem;
}

/* Responsive table */
@media (max-width: 768px) {
  .table-responsive {
    font-size: 0.875rem;
  }
}
</style>