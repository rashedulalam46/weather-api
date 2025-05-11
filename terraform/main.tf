module "resource_group" {
  source   = "./modules/networking"
  location = var.location
  name     = "${var.project_name}-rg"
}

module "aks_cluster" {
  source              = "./modules/aks"
  resource_group_name = module.resource_group.name
  location           = var.location
  cluster_name       = "${var.project_name}-aks"
  node_count         = var.node_count
  vm_size            = var.vm_size
}

module "monitoring" {
  source              = "./modules/monitoring"
  resource_group_name = module.resource_group.name
  location           = var.location
  cluster_name       = module.aks_cluster.name
}