variable "region" {
  description = "The AWS region to deploy resources"
  default     = "eu-central-1"
}

variable "instance_type" {
  description = "Instance type for Lightsail"
  default     = "nano_2_0"
}

variable "db_username" {
  description = "The administrator username for the SQL server"
  default     = "sa"
}

variable "db_password" {
  description = "The administrator password for the SQL server"
  default     = "Hakerman91!"
}

variable "github_username" {
  description = "Your GitHub username for accessing GitHub Container Registry"
  default = "takerman"
}

variable "github_token" {
  description = "Your GitHub personal access token for accessing GitHub Container Registry"
  default = "hp_wtwkBh5xiWHBQn3qFp2WGExdmAE9vB2cOrcW"
}
