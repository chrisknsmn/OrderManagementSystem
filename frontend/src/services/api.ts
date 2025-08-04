import axios from 'axios'
import type {
  Customer,
  Vehicle,
  RepairOrder,
  CreateRepairOrderRequest
} from '../types'

const api = axios.create({
  baseURL: '/api',
  headers: {
    'Content-Type': 'application/json',
  },
  timeout: 10000, // 10 second timeout
})

// Add response interceptor for better error handling
api.interceptors.response.use(
  response => response,
  error => {
    console.error('API Error:', error.response?.data || error.message)
    return Promise.reject(error)
  }
)

// Customer API
export const customerApi = {
  getAll: (): Promise<Customer[]> => api.get('/customers').then(res => res.data),
  getById: (id: number): Promise<Customer> => api.get(`/customers/${id}`).then(res => res.data),
  create: (customer: Omit<Customer, 'id'>): Promise<Customer> => api.post('/customers', customer).then(res => res.data),
  delete: (id: number): Promise<void> => api.delete(`/customers/${id}`).then(res => res.data),
}

// Vehicle API
export const vehicleApi = {
  getAll: (): Promise<Vehicle[]> => api.get('/vehicles').then(res => res.data),
  getById: (id: number): Promise<Vehicle> => api.get(`/vehicles/${id}`).then(res => res.data),
  create: (vehicle: Omit<Vehicle, 'id'>): Promise<Vehicle> => api.post('/vehicles', vehicle).then(res => res.data),
  delete: (id: number): Promise<void> => api.delete(`/vehicles/${id}`).then(res => res.data),
}

// Repair Order API
export const repairOrderApi = {
  getAll: (): Promise<RepairOrder[]> => api.get('/repairorders').then(res => res.data),
  getById: (id: number): Promise<RepairOrder> => api.get(`/repairorders/${id}`).then(res => res.data),
  create: (order: CreateRepairOrderRequest): Promise<RepairOrder> => api.post('/repairorders', order).then(res => res.data),
  delete: (id: number): Promise<void> => api.delete(`/repairorders/${id}`).then(res => res.data),
  getByStatus: (status: string): Promise<RepairOrder[]> => api.get(`/repairorders/status/${status}`).then(res => res.data),
}


export default api