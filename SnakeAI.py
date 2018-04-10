import unityagents
from unityagents import UnityEnvironment
import matplotlib as plt
import numpy as np



env_name = "SnakeAI"
train_mode = True
env = UnityEnvironment(file_name=env_name)
default_brain = env.brain_names[0]
brain = env.brains[default_brain]

print(str(env))
env.reset(train_mode=train_mode, config=None)

env.close()


