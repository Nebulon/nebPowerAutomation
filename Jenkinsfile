node {
    def currentV
    def targetV

    // setup defaults
    currentBuild.result = 'SUCCESS'

    try {

        stage('Clean Workspace') {
            cleanWs()
        }

        stage('Checkout BitBucket') {
            dir("bitbucket") {
                git(
                    branch: 'master',
                    credentialsId: 'bitbucket.tobias.ssh',
                    url: 'git@bitbucket.org:nebuloninc/nebpowershell.git'
                )

                targetV = powershell (returnStdout: true, script: "Get-Content VERSION")
                targetV = targetV.trim()
                targetV = targetV.replaceAll("(\r\n)", "")
                bat "echo 'Target version is ${targetV}'"

                env.TARGET = targetV
            }
        }

        stage('Checkout GitHub') {
            withCredentials([string(credentialsId: 'github-powershell', variable: 'GITHUB_TOKEN')]) {
                bat "git clone https://token:${GITHUB_TOKEN}@github.com/Nebulon/nebPowerAutomation.git"

                currentV = powershell (returnStdout: true, script: "Get-Content nebPowerAutomation\\VERSION")
                currentV = currentV.trim()
                currentV = currentV.replaceAll("(\r\n)", "")
                bat "echo 'Current version is ${currentV}'"

                env.CURRENT = currentV
            }
        }

       
        stage('Update GitHub') {
            withEnv(["TARGET=${targetV}","CURRENT=${currentV}"]) {
                bat "echo 'Current: ${env.CURRENT}, Target: ${env.TARGET}'"

                if (env.CURRENT != env.TARGET) {
                    bat "cd nebPowerAutomation && dir"
                    bat "cd nebPowerAutomation && git config user.name tflitsch"
                    bat "cd nebPowerAutomation && git config user.email tflitsch@users.noreply.github.com"

                    bat "xcopy bitbucket nebPowerAutomation /s /e /y"
                    bat "cd nebPowerAutomation && git add ."
                    bat "cd nebPowerAutomation && git status"
                    bat "cd nebPowerAutomation && git commit -m \"Repository sync\""
                    bat "cd nebPowerAutomation && git tag v${env.TARGET}"
                    bat "cd nebPowerAutomation && git push"
                    bat "cd nebPowerAutomation && git push --tags"

                } else {
                    bat "echo 'Version is already current. Won't sync with GitHub'"
                }

                bat "rd /S /Q nebPowerAutomation"
            }
        }
        

        stage('Build') {
            withEnv(["TARGET=${targetV}","CURRENT=${currentV}"]) {
                dir("bitbucket") {
                    bat "dir"
                    bat "nuget.exe restore NebPowerAutomation.sln"
                    bat "MSBuild.exe -verbosity:q NebPowerAutomation.sln -property:Configuration=Release -p:AssemblyVersionNumber=${env.TARGET} -p:AssemblyInformationalVersion=${env.TARGET}"
                }
            }
        }

        stage('Package') {
            withEnv(["TARGET=${targetV}","CURRENT=${currentV}"]) {
                bat "dir"
                bat "mkdir NebPowerAutomation"
                bat "xcopy bitbucket\\NebPowerAutomation\\bin\\Release\\*.dll NebPowerAutomation /D /S /Y"
                bat "xcopy bitbucket\\NebPowerAutomation\\bin\\Release\\*.xml NebPowerAutomation /D /S /Y"
                bat "xcopy bitbucket\\NebPowerAutomation\\*.psd1 NebPowerAutomation /D /S /Y"
                powershell "Update-ModuleManifest -Path .\\NebPowerAutomation\\NebPowerAutomation.psd1 -ModuleVersion \"${env.TARGET}\""
                powershell 'Test-ModuleManifest -Path ".\\NebPowerAutomation\\NebPowerAutomation.psd1"'
            }
        }

        stage('Publish PowerShellGallery') {
            if (publish == "true") {
                withCredentials([string(credentialsId: 'powershell-gallery', variable: 'PSG_TOKEN')]) {
                    powershell "Publish-Module  -Path .\\NebPowerAutomation -NugetAPIKey ${PSG_TOKEN} -Force"
                }
            }
        }

    } catch(e) {
        currentBuild.result = 'FAILED'
        notifySlack('FAILED', '')
    }

    if (currentBuild.result == 'SUCCESS') {
        echo 'Build succeeded'
        notifySlack('SUCCESS', 'Install with `Install-Module -Name NebPowerAutomation`')
    }
}

def notifySlack(String status, String message) {
    def subject = "*${status}*: PowerShell Module v${env.TARGET}"

    def links = "<${env.BUILD_URL}console|Console Output>"
    def summary = "${subject}\n${links}\n${message}"

    def colorCode = '#FF0000'
    if (status == 'SUCCESS') {
        colorCode = '#00FF00'
    }

    slackSend (channel: '#sdks', color: colorCode, message: summary)
}
