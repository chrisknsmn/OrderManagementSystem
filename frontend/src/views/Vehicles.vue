<template>
  <div>
    <div class="d-flex justify-content-between align-items-center mb-4">
      <h1 class="h2">
        <i class="bi bi-car-front text-primary"></i>
        Vehicles
      </h1>
      <button class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#addVehicleModal">
        <i class="bi bi-plus-circle"></i>
        Add Vehicle
      </button>
    </div>

    <!-- Loading State -->
    <div v-if="loading && vehicles.length === 0" class="text-center py-5">
      <div class="spinner-border text-primary" role="status">
        <span class="visually-hidden">Loading...</span>
      </div>
    </div>

    <!-- Error State -->
    <div v-else-if="error" class="alert alert-danger" role="alert">
      <i class="bi bi-exclamation-triangle"></i>
      {{ error }}
    </div>

    <!-- Vehicles List -->
    <div v-else>
      <div class="row">
        <div v-for="vehicle in vehicles" :key="vehicle.id" class="col-lg-4 col-md-6 mb-4">
          <div class="card h-100">
            <div class="card-body">
              <h5 class="card-title">
                <i class="bi bi-car-front-fill text-primary"></i>
                {{ vehicle.year }} {{ vehicle.make }}
              </h5>
              <h6 class="card-subtitle mb-2 text-muted">{{ vehicle.model }}</h6>
              <div class="d-flex justify-content-between align-items-center mt-3">
                <button
                  class="btn btn-outline-danger btn-sm"
                  @click="deleteVehicle(vehicle)"
                  :disabled="deleting"
                >
                  <i class="bi bi-trash"></i>
                  Delete
                </button>
                <small class="text-muted">ID: {{ vehicle.id }}</small>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Empty State -->
      <div v-if="vehicles.length === 0" class="text-center py-5">
        <i class="bi bi-car-front fs-1 text-muted"></i>
        <h4 class="text-muted mt-3">No vehicles found</h4>
        <p class="text-muted">Add your first vehicle to get started.</p>
      </div>
    </div>

    <!-- Add Vehicle Modal -->
    <div class="modal fade" id="addVehicleModal" tabindex="-1" aria-labelledby="addVehicleModalLabel" aria-hidden="true">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="addVehicleModalLabel">
              <i class="bi bi-car-front-fill"></i>
              Add New Vehicle
            </h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
          </div>
          <form @submit.prevent="createVehicle">
            <div class="modal-body">
              <div class="mb-3">
                <label for="year" class="form-label">Year *</label>
                <input 
                  type="number" 
                  class="form-control" 
                  id="year" 
                  v-model.number="newVehicle.year" 
                  required
                  :min="1900"
                  :max="new Date().getFullYear() + 1"
                >
              </div>
              <div class="mb-3">
                <label for="make" class="form-label">Make *</label>
                <input 
                  type="text" 
                  class="form-control" 
                  id="make" 
                  v-model="newVehicle.make" 
                  required
                  placeholder="Toyota, Honda, Ford..."
                >
              </div>
              <div class="mb-3">
                <label for="model" class="form-label">Model *</label>
                <input 
                  type="text" 
                  class="form-control" 
                  id="model" 
                  v-model="newVehicle.model" 
                  required
                  placeholder="Camry, Civic, F-150..."
                >
              </div>
            </div>
            <div class="modal-footer">
              <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
              <button type="submit" class="btn btn-primary" :disabled="creating">
                <span v-if="creating" class="spinner-border spinner-border-sm me-2" role="status"></span>
                {{ creating ? 'Creating...' : 'Create Vehicle' }}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>

  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { vehicleApi } from '../services/api'
import type { Vehicle } from '../types'

const vehicles = ref<Vehicle[]>([])
const loading = ref(false)
const error = ref<string | null>(null)
const creating = ref(false)
const deleting = ref(false)

const newVehicle = ref({
  year: new Date().getFullYear(),
  make: '',
  model: ''
})

const fetchVehicles = async () => {
  loading.value = true
  error.value = null
  
  try {
    vehicles.value = await vehicleApi.getAll()
  } catch (err) {
    error.value = 'Failed to load vehicles. Please try again.'
    console.error('Vehicles error:', err)
  } finally {
    loading.value = false
  }
}

const createVehicle = async () => {
  creating.value = true
  
  try {
    const vehicle = await vehicleApi.create(newVehicle.value)
    vehicles.value.push(vehicle)
    
    // Reset form and close modal
    newVehicle.value = { year: new Date().getFullYear(), make: '', model: '' }
    const modal = document.getElementById('addVehicleModal')
    if (modal && (window as any).bootstrap?.Modal) {
      const bsModal = (window as any).bootstrap.Modal.getInstance(modal)
      if (bsModal) {
        bsModal.hide()
      }
    }
  } catch (err) {
    error.value = 'Failed to create vehicle. Please try again.'
    console.error('Create vehicle error:', err)
  } finally {
    creating.value = false
  }
}

const deleteVehicle = async (vehicle: Vehicle) => {
  if (!confirm(`Are you sure you want to delete vehicle "${vehicle.year} ${vehicle.make} ${vehicle.model}"? This will also delete all related repair orders.`)) {
    return;
  }

  deleting.value = true;
  try {
    await vehicleApi.delete(vehicle.id);
    
    // Remove the vehicle from the list
    const index = vehicles.value.findIndex(v => v.id === vehicle.id);
    if (index !== -1) {
      vehicles.value.splice(index, 1);
    }
  } catch (err) {
    error.value = 'Failed to delete vehicle. Please try again.';
    console.error('Delete vehicle error:', err);
  } finally {
    deleting.value = false;
  }
};

onMounted(() => {
  fetchVehicles()
})
</script>