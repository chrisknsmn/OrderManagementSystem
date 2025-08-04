<template>
  <div>
    <div class="d-flex justify-content-between align-items-center mb-4">
      <h1 class="h2">
        <i class="bi bi-people text-primary"></i>
        Customers
      </h1>
      <button
        class="btn btn-primary"
        data-bs-toggle="modal"
        data-bs-target="#addCustomerModal"
      >
        <i class="bi bi-plus-circle"></i>
        Add Customer
      </button>
    </div>

    <!-- Loading State -->
    <div v-if="loading && customers.length === 0" class="text-center py-5">
      <div class="spinner-border text-primary" role="status">
        <span class="visually-hidden">Loading...</span>
      </div>
    </div>

    <!-- Error State -->
    <div v-else-if="error" class="alert alert-danger" role="alert">
      <i class="bi bi-exclamation-triangle"></i>
      {{ error }}
    </div>

    <!-- Customers List -->
    <div v-else>
      <div class="row">
        <div
          v-for="customer in customers"
          :key="customer.id"
          class="col-lg-4 col-md-6 mb-4"
        >
          <div class="card h-100">
            <div class="card-body">
              <h5 class="card-title">
                <i class="bi bi-person-circle text-primary"></i>
                {{ customer.firstName }} {{ customer.lastName }}
              </h5>
              <p class="card-text">
                <i class="bi bi-telephone"></i>
                {{ customer.phoneNumber }}
              </p>
              <div class="d-flex justify-content-between align-items-center">
                <button
                  class="btn btn-outline-danger btn-sm"
                  @click="deleteCustomer(customer)"
                  :disabled="deleting"
                >
                  <i class="bi bi-trash"></i>
                  Delete
                </button>
                <small class="text-muted">ID: {{ customer.id }}</small>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Empty State -->
      <div v-if="customers.length === 0" class="text-center py-5">
        <i class="bi bi-people fs-1 text-muted"></i>
        <h4 class="text-muted mt-3">No customers found</h4>
        <p class="text-muted">Add your first customer to get started.</p>
      </div>
    </div>

    <!-- Add Customer Modal -->
    <div
      class="modal fade"
      id="addCustomerModal"
      tabindex="-1"
      aria-labelledby="addCustomerModalLabel"
      aria-hidden="true"
    >
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="addCustomerModalLabel">
              <i class="bi bi-person-plus"></i>
              Add New Customer
            </h5>
            <button
              type="button"
              class="btn-close"
              data-bs-dismiss="modal"
              aria-label="Close"
            ></button>
          </div>
          <form @submit.prevent="createCustomer">
            <div class="modal-body">
              <div class="mb-3">
                <label for="firstName" class="form-label">First Name *</label>
                <input
                  type="text"
                  class="form-control"
                  id="firstName"
                  v-model="newCustomer.firstName"
                  required
                />
              </div>
              <div class="mb-3">
                <label for="lastName" class="form-label">Last Name *</label>
                <input
                  type="text"
                  class="form-control"
                  id="lastName"
                  v-model="newCustomer.lastName"
                  required
                />
              </div>
              <div class="mb-3">
                <label for="phoneNumber" class="form-label"
                  >Phone Number *</label
                >
                <input
                  type="tel"
                  class="form-control"
                  id="phoneNumber"
                  v-model="newCustomer.phoneNumber"
                  required
                  placeholder="555-123-4567"
                />
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
                {{ creating ? "Creating..." : "Create Customer" }}
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
import { customerApi } from "../services/api";
import type { Customer } from "../types";

const customers = ref<Customer[]>([]);
const loading = ref(false);
const error = ref<string | null>(null);
const creating = ref(false);
const deleting = ref(false);

const newCustomer = ref({
  firstName: "",
  lastName: "",
  phoneNumber: "",
});

const fetchCustomers = async () => {
  loading.value = true;
  error.value = null;

  try {
    customers.value = await customerApi.getAll();
  } catch (err) {
    error.value = "Failed to load customers. Please try again.";
    console.error("Customers error:", err);
  } finally {
    loading.value = false;
  }
};

const createCustomer = async () => {
  creating.value = true;

  try {
    const customer = await customerApi.create(newCustomer.value);
    customers.value.push(customer);

    // Reset form and close modal
    newCustomer.value = { firstName: "", lastName: "", phoneNumber: "" };
    const modal = document.getElementById("addCustomerModal");
    if (modal && (window as any).bootstrap?.Modal) {
      const bsModal = (window as any).bootstrap.Modal.getInstance(modal);
      if (bsModal) {
        bsModal.hide();
      }
    }
  } catch (err) {
    error.value = "Failed to create customer. Please try again.";
    console.error("Create customer error:", err);
  } finally {
    creating.value = false;
  }
};


const deleteCustomer = async (customer: Customer) => {
  if (!confirm(`Are you sure you want to delete customer "${customer.firstName} ${customer.lastName}"? This will also delete all their repair orders.`)) {
    return;
  }

  deleting.value = true;
  try {
    await customerApi.delete(customer.id);
    
    // Remove the customer from the list
    const index = customers.value.findIndex(c => c.id === customer.id);
    if (index !== -1) {
      customers.value.splice(index, 1);
    }
  } catch (err) {
    error.value = "Failed to delete customer. Please try again.";
    console.error("Delete customer error:", err);
  } finally {
    deleting.value = false;
  }
};

const formatDate = (dateString: string) => {
  return new Date(dateString).toLocaleDateString();
};

onMounted(() => {
  fetchCustomers();
});
</script>
