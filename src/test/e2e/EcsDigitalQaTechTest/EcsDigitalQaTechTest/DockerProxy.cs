using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace EcsDigitalQaTechTest
{
    public class DockerProxy
    {
        private readonly DockerClient dockerClient;
        private const string defaultWindowsDockerUrl = "npipe://./pipe/docker_engine";
        private const int maxNumContainersReturned = 10;
        private const int maxWaitTimeContainerKill = 30;

        public DockerProxy()
        {
            dockerClient = new DockerClientConfiguration(
                    new Uri(defaultWindowsDockerUrl)).CreateClient();
        }

        public List<ContainerListResponse> GetContainersByImageName(string imageName)
        {
            var containers = dockerClient.Containers.ListContainersAsync(
                new ContainersListParameters()
                {
                    Limit = maxNumContainersReturned,
                }).Result;
            var digitalTestContainers =
                containers.Where(c => c.Image.Equals(imageName)).ToList();
            return digitalTestContainers;
        }

        public async Task StartContainer(string containerName)
        {
            var digitalTestContainerListResponses = GetContainersByImageName(containerName);
            var firstContainer = digitalTestContainerListResponses[0];
            await dockerClient.Containers.StartContainerAsync(firstContainer.ID,
                new ContainerStartParameters());
        }

        public async Task StopContainer(string containerName)
        {
            var digitalTestContainerListResponses = GetContainersByImageName(containerName);
            var firstContainer = digitalTestContainerListResponses[0];
            await dockerClient.Containers.StopContainerAsync(firstContainer.ID,
                new ContainerStopParameters
                {
                    WaitBeforeKillSeconds = (uint)maxWaitTimeContainerKill
                },
                CancellationToken.None);
        }
    }
}
