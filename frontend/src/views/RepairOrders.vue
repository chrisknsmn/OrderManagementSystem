<template>
  <div>
    <div class="d-flex justify-content-between align-items-center mb-4">
      <h1 class="h2">
        <i class="bi bi-clipboard-check text-primary"></i>
        Repair Orders
      </h1>
      <button
        class="btn btn-primary"
        data-bs-toggle="modal"
        data-bs-target="#addRepairOrderModal"
      >
        <i class="bi bi-plus-circle"></i>
        Create Order
      </button>
    </div>

    <!-- Filters -->
    <div class="row mb-4">
      <div>
        <select
          class="form-select"
          v-model="selectedStatus"
          @change="filterByStatus"
        >
          <option value="">All Statuses</option>
          <option value="Open">Open</option>
          <option value="In Progress">In Progress</option>
          <option value="Completed">Completed</option>
          <option value="Cancelled">Cancelled</option>
        </select>
      </div>
    </div>

    <!-- Loading State -->
    <div v-if="loading && repairOrders.length === 0" class="text-center py-5">
      <div class="spinner-border text-primary" role="status">
        <span class="visually-hidden">Loading...</span>
      </div>
    </div>

    <!-- Error State -->
    <div v-else-if="error" class="alert alert-danger" role="alert">
      <i class="bi bi-exclamation-triangle"></i>
      {{ error }}
    </div>

    <!-- Repair Orders Table -->
    <div v-else class="card">
      <div class="card-body">
        <div v-if="repairOrders.length === 0" class="text-center py-5">
          <i class="bi bi-clipboard-x fs-1 text-muted"></i>
          <h4 class="text-muted mt-3">No repair orders found</h4>
          <p class="text-muted">
            Create your first repair order to get started.
          </p>
        </div>
        <div v-else class="table-responsive">
          <table class="table table-hover">
            <thead>
              <tr>
                <th>Order #</th>
                <th>Customer</th>
                <th>Vehicle</th>
                <th>Description</th>
                <th>Cost</th>
                <th>Status</th>
                <th>Created</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="order in repairOrders" :key="order.id">
                <td class="fw-bold">#{{ order.id }}</td>
                <td>
                  <i class="bi bi-person-circle text-muted"></i>
                  {{ order.customer?.firstName }} {{ order.customer?.lastName }}
                </td>
                <td>
                  <i class="bi bi-car-front text-muted"></i>
                  {{ order.vehicle?.year }} {{ order.vehicle?.make }}
                  {{ order.vehicle?.model }}
                </td>
                <td>
                  <span
                    class="text-truncate d-inline-block"
                    style="max-width: 200px"
                  >
                    {{ order.description }}
                  </span>
                </td>
                <td class="text-success fw-bold">
                  ${{ order.estimatedCost.toFixed(2) }}
                </td>
                <td>
                  <span
                    class="badge"
                    :class="getStatusBadgeClass(order.status)"
                  >
                    {{ order.status }}
                  </span>
                </td>
                <td>{{ formatDate(order.createdDate) }}</td>
                <td>
                  <button
                    class="btn btn-outline-danger btn-sm"
                    @click="deleteOrder(order)"
                    :disabled="deleting"
                  >
                    <i class="bi bi-trash"></i>
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>

    <!-- Create Repair Order Modal -->
    <div
      class="modal fade"
      id="addRepairOrderModal"
      tabindex="-1"
      aria-labelledby="addRepairOrderModalLabel"
      aria-hidden="true"
    >
      <div class="modal-dialog modal-lg">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="addRepairOrderModalLabel">
              <i class="bi bi-clipboard-plus"></i>
              Create New Repair Order
            </h5>
            <button
              type="button"
              class="btn-close"
              data-bs-dismiss="modal"
              aria-label="Close"
            ></button>
          </div>
          <form @submit.prevent="createRepairOrder">
            <div class="modal-body">
              <div class="row">
                <div class="col-md-6">
                  <div class="mb-3">
                    <label for="customerId" class="form-label"
                      >Customer *</label
                    >
                    <select
                      class="form-select"
                      id="customerId"
                      v-model="newRepairOrder.customerId"
                      required
                    >
                      <option value="">Select a customer...</option>
                      <option
                        v-for="customer in customers"
                        :key="customer.id"
                        :value="customer.id"
                      >
                        {{ customer.firstName }} {{ customer.lastName }} -
                        {{ customer.phoneNumber }}
                      </option>
                    </select>
                  </div>
                </div>
                <div class="col-md-6">
                  <div class="mb-3">
                    <label for="vehicleId" class="form-label">Vehicle *</label>
                    <select
                      class="form-select"
                      id="vehicleId"
                      v-model="newRepairOrder.vehicleId"
                      required
                    >
                      <option value="">Select a vehicle...</option>
                      <option
                        v-for="vehicle in vehicles"
                        :key="vehicle.id"
                        :value="vehicle.id"
                      >
                        {{ vehicle.year }} {{ vehicle.make }}
                        {{ vehicle.model }}
                      </option>
                    </select>
                  </div>
                </div>
              </div>
              <div class="mb-3">
                <label for="description" class="form-label"
                  >Description *</label
                >
                <textarea
                  class="form-control"
                  id="description"
                  rows="3"
                  v-model="newRepairOrder.description"
                  required
                  placeholder="Describe the repair work needed..."
                ></textarea>
              </div>
              <div class="mb-3">
                <label for="estimatedCost" class="form-label"
                  >Estimated Cost *</label
                >
                <div class="input-group">
                  <span class="input-group-text">$</span>
                  <input
                    type="number"
                    class="form-control"
                    id="estimatedCost"
                    v-model.number="newRepairOrder.estimatedCost"
                    required
                    min="0"
                    step="0.01"
                    placeholder="0.00"
                  />
                </div>
              </div>
            </div>
            <div class="modal-footer">
              <button
                type="button"
                class="btn btn-secondary"
                data-bs-dismiss="modal"
              >
                Cancel
              </button>
              <button
                type="submit"
                class="btn btn-primary"
                :disabled="creating"
              >
                <span
                  v-if="creating"
                  class="spinner-border spinner-border-sm me-2"
                  role="status"
                ></span>
                {{ creating ? "Creating..." : "Create Order" }}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>

  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
import { repairOrderApi, customerApi, vehicleApi } from "../services/api";
import type {
  RepairOrder,
  Customer,
  Vehicle,
  CreateRepairOrderRequest,
} from "../types";

const repairOrders = ref<RepairOrder[]>([]);
const customers = ref<Customer[]>([]);
const vehicles = ref<Vehicle[]>([]);
const loading = ref(false);
const error = ref<string | null>(null);
const creating = ref(false);
const deleting = ref(false);
const selectedStatus = ref<string>("");

const newRepairOrder = ref<CreateRepairOrderRequest>({
  customerId: 0,
  vehicleId: 0,
  description: "",
  estimatedCost: 0,
});

const fetchData = async () => {
  loading.value = true;
  error.value = null;

  try {
    const [ordersData, customersData, vehiclesData] = await Promise.all([
      repairOrderApi.getAll(),
      customerApi.getAll(),
      vehicleApi.getAll(),
    ]);

    repairOrders.value = ordersData;
    customers.value = customersData;
    vehicles.value = vehiclesData;
  } catch (err) {
    error.value = "Failed to load data. Please try again.";
    console.error("Data loading error:", err);
  } finally {
    loading.value = false;
  }
};

const createRepairOrder = async () => {
  creating.value = true;

  try {
    const order = await repairOrderApi.create(newRepairOrder.value);
    repairOrders.value.unshift(order);

    // Reset form and close modal
    newRepairOrder.value = {
      customerId: 0,
      vehicleId: 0,
      description: "",
      estimatedCost: 0,
    };
    const modal = document.getElementById("addRepairOrderModal");
    if (modal && (window as any).bootstrap?.Modal) {
      const bsModal = (window as any).bootstrap.Modal.getInstance(modal);
      if (bsModal) {
        bsModal.hide();
      }
    }
  } catch (err) {
    error.value = "Failed to create repair order. Please try again.";
    console.error("Create order error:", err);
  } finally {
    creating.value = false;
  }
};

const filterByStatus = async () => {
  if (!selectedStatus.value) {
    await fetchData();
    return;
  }

  loading.value = true;
  try {
    repairOrders.value = await repairOrderApi.getByStatus(selectedStatus.value);
  } catch (err) {
    error.value = "Failed to filter orders. Please try again.";
    console.error("Filter error:", err);
  } finally {
    loading.value = false;
  }
};

const deleteOrder = async (order: RepairOrder) => {
  if (!confirm(`Are you sure you want to delete repair order #${order.id}?`)) {
    return;
  }

  deleting.value = true;
  try {
    await repairOrderApi.delete(order.id);
    
    // Remove the order from the list
    const index = repairOrders.value.findIndex(o => o.id === order.id);
    if (index !== -1) {
      repairOrders.value.splice(index, 1);
    }
  } catch (err) {
    error.value = "Failed to delete repair order. Please try again.";
    console.error("Delete order error:", err);
  } finally {
    deleting.value = false;
  }
};

const getStatusBadgeClass = (status: string) => {
  switch (status.toLowerCase()) {
    case "completed":
      return "bg-success";
    case "in progress":
      return "bg-warning";
    case "open":
      return "bg-primary";
    case "cancelled":
      return "bg-danger";
    default:
      return "bg-secondary";
  }
};

const formatDate = (dateString: string) => {
  return new Date(dateString).toLocaleDateString();
};

onMounted(() => {
  fetchData();
});
</script>
