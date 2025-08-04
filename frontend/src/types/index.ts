export interface Customer {
  id: number
  firstName: string
  lastName: string
  phoneNumber: string
  repairOrders?: RepairOrder[]
}

export interface Vehicle {
  id: number
  year: number
  make: string
  model: string
  repairOrders?: RepairOrder[]
}

export interface RepairOrder {
  id: number
  customerId: number
  vehicleId: number
  createdDate: string
  description: string
  estimatedCost: number
  status: string
  customer?: Customer
  vehicle?: Vehicle
}

export interface CreateRepairOrderRequest {
  customerId: number
  vehicleId: number
  description: string
  estimatedCost: number
}


