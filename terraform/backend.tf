terraform {
  backend "azurerm" {
    resource_group_name  = "tfstate-rg"
    storage_account_name = "tfstate${substr(md5(var.project_name), 0, 8)}"
    container_name       = "tfstate"
    key                  = "prod.terraform.tfstate"
  }
}