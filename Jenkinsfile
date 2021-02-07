pipeline {
    agent any

    stages {  
        stage('Checkout'){
            steps {
                  git branch: env.branch,
                    credentialsId: 'bitbucket.tobias.ssh',
                    url: "git@bitbucket.org:nebuloninc/nebpowershell.git"
            }
        }
        stage("Env Variables") {
            steps {
                echo bat(returnStdout: true, script: 'set')
            }
        }
        stage('Build'){
            environment {
                app_version = getAppVersion()
                NUGET_PACKAGES = getNugetDir()
            }
            steps {
                echo "Author: ${env.CHANGE_AUTHOR}"
                echo "Tag: ${env.TAG_NAME}"
                bat "nuget.exe restore -Verbosity quiet"
                bat "MSBuild.exe NebPowerAutomation.sln /property:Configuration=%configuration% /verbosity:quiet "
            }
        }
        stage('Test'){
            environment{
                NEB_CREDENTIALS = credentials('nebcloud-credentials')
            }
            steps {
                bat "\"C:\\Program Files (x86)\\Microsoft Visual Studio\\2019\\BuildTools\\Common7\\IDE\\CommonExtensions\\Microsoft\\TestWindow\\vstest.console.exe\" NebSharpTest\\bin\\%configuration%\\netcoreapp3.1\\NebSharpTest.dll /Logger:html;LogFileName=TestResults.html"
            }
        }
        stage('Package') {
            steps {
                bat "tar -c -a -f package.zip -C NebPowerAutomation\\bin\\%configuration% *.dll"
            }
        }
    }
    post { 
        always { 
            archiveArtifacts artifacts: 'package.zip,TestResults\\TestResults.html', fingerprint: true
            notifySlack(bat(returnStdout: true, script: 'git log --pretty=format:"%h - %an, %ar : %s" --since=1.days'))
            cleanWs()
        }
    }
}

def getNugetDir() {
    return pwd() + "/" + "packages"
}

def getAppVersion() {
    return BUILD_ID
}

def notifySlack(changes) {
  def status = currentBuild.currentResult  
  def subject = "*${status}*: Job `${env.JOB_NAME} [${env.BUILD_NUMBER}]` Branch `${env.branch}`"
  def linksOK = "<${env.BUILD_URL}console|Console Output> - <${env.BUILD_URL}artifact/TestResults/TestResults.html|Test Results> - <${env.BUILD_URL}artifact/package.zip|Package>"
  def linksFailure = "<${env.BUILD_URL}|Job URL> - <${env.BUILD_URL}console|Console Output> - <${env.BUILD_URL}artifact/TestResults/TestResults.html|Test Results>\n${changes}"
  def summary = "${subject}\n${linksFailure}"
  
  def color = 'RED'
  def colorCode = '#FF0000'

  if (status == 'UNSTABLE') {
    color = 'YELLOW'
    colorCode = '##FFFF00'
  } else if (status == 'SUCCESS') {
    color = 'GREEN'
    colorCode = '#00FF00'
    summary = "${subject}\n${linksOK}"
  }
  
  slackSend (color: colorCode, message: summary)
}

