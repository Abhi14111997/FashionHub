pipeline {
    agent any

    environment {
        DOCKER_IMAGE = 'fashionhub'
        DOCKER_TAG = "${BUILD_NUMBER}"
        CONTAINER_NAME = 'fashionhub-app'
        APP_PORT = '5000'
    }

    stages {

        stage('📥 Checkout') {
            steps {
                echo '=== Pulling latest code from GitHub ==='
                checkout scm
            }
        }

        stage('🔍 Code Quality Check') {
            steps {
                echo '=== Checking .NET build warnings ==='
                sh 'dotnet restore FashionHub.csproj'
                sh 'dotnet build FashionHub.csproj -c Release --no-restore 2>&1 | tee build.log'
            }
        }

        stage('🧪 Run Tests') {
            steps {
                echo '=== Running tests (add test project later) ==='
                sh 'echo "No tests yet - Add xUnit project later"'
            }
        }

        stage('🐳 Build Docker Image') {
            steps {
                echo '=== Building Docker Image ==='
                sh "docker build -t ${DOCKER_IMAGE}:${DOCKER_TAG} ."
                sh "docker tag ${DOCKER_IMAGE}:${DOCKER_TAG} ${DOCKER_IMAGE}:latest"
                echo "✅ Docker image built: ${DOCKER_IMAGE}:${DOCKER_TAG}"
            }
        }

        stage('🚀 Deploy Container') {
            steps {
                echo '=== Stopping old container (if running) ==='
                sh "docker stop ${CONTAINER_NAME} || true"
                sh "docker rm ${CONTAINER_NAME} || true"

                echo '=== Starting new container ==='
                sh """
                    docker run -d \
                        --name ${CONTAINER_NAME} \
                        -p ${APP_PORT}:80 \
                        -e ASPNETCORE_ENVIRONMENT=Production \
                        -e ConnectionStrings__DefaultConnection='Server=host.docker.internal;Database=FashionHubDB;User Id=sa;Password=YourPass123!;TrustServerCertificate=True' \
                        --restart unless-stopped \
                        ${DOCKER_IMAGE}:latest
                """
                echo "✅ App running on http://YOUR-SERVER-IP:${APP_PORT}"
            }
        }

        stage('🏥 Health Check') {
            steps {
                echo '=== Waiting for app to start ==='
                sh 'sleep 10'
                sh "curl -f http://localhost:${APP_PORT} || echo 'Health check failed - check logs'"
                sh "docker ps | grep ${CONTAINER_NAME}"
            }
        }

        stage('🧹 Cleanup Old Images') {
            steps {
                echo '=== Removing old Docker images ==='
                sh "docker image prune -f"
                echo '✅ Cleanup done'
            }
        }
    }

    post {
        success {
            echo '🎉 ===== BUILD & DEPLOY SUCCESSFUL ====='
            echo "🌐 Website live at: http://YOUR-SERVER-IP:${APP_PORT}"
        }
        failure {
            echo '❌ ===== BUILD FAILED - Check logs above ====='
        }
        always {
            echo "📋 Build #${BUILD_NUMBER} completed"
        }
    }
}
