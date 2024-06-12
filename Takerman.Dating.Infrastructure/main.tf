provider "aws" {
  region = "eu-west-3"
}

resource "aws_lightsail_instance" "app" {
  name              = "takerman-dating"
  availability_zone = "eu-west-3a"
  blueprint_id      = "amazon_linux_2"
  bundle_id         = "nano_2_0"

  user_data = <<-EOF
              #!/bin/bash
              yum update -y
              amazon-linux-extras install docker -y
              service docker start
              usermod -a -G docker ec2-user
              docker login ghcr.io -u GITHUB_USERNAME -p GITHUB_TOKEN
              docker run -d --name takerman-dating -p 80:80 ghcr.io/takermanltd/app-dating:latest
              EOF
}

resource "aws_lightsail_static_ip" "app_ip" {
  name = "takerman-dating-ip"
}

resource "aws_lightsail_static_ip_attachment" "app_ip_attachment" {
  instance_name = aws_lightsail_instance.app.name
  static_ip_name = aws_lightsail_static_ip.app_ip.name
}

resource "aws_db_instance" "sql" {
  identifier              = "takerman-dating-bg"
  allocated_storage       = 20
  engine                  = "sqlserver-ex"
  instance_class          = "db.t3.micro"
  username                = "sa"
  password                = "Hakerman91!"
  skip_final_snapshot     = true
  publicly_accessible     = true
  backup_retention_period = 0

  tags = {
    Name = "takerman-dating-bg"
  }
}
