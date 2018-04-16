from unityagents import UnityEnvironment
from unityagents import BrainInfo
from unityagents import AllBrainInfo
print('Imported')



env_name = "SnakeAI"
train_mode = True
env = UnityEnvironment(file_name=env_name)
default_brain = env.brain_names[0]
brain = env.brains[default_brain]
env.reset()
env.step(1)


print(str(env))
print('__________')
print(str(BrainInfo))
env.reset(train_mode=train_mode, config=None)

env.close()


