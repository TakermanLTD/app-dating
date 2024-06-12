output "app_public_ip" {
  value = aws_lightsail_static_ip.app_ip.ip_address
}

output "sql_endpoint" {
  value = aws_db_instance.sql.endpoint
}

output "sql_instance_class" {
  value = aws_db_instance.sql.instance_class
}
