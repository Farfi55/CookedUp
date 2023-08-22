import os
import os.path
import platform

def join_path(*args):
    return os.path.join(*args)
sep = os.path.sep

project_path = __file__.split(join_path('Assets','Scripts'))[0]
base_streaming_assets_path = join_path(project_path, 'Assets', 'StreamingAssets', '')
think_engine_streaming_assets_path = join_path(base_streaming_assets_path,'ThinkEngineer','ThinkEngine', '')
solvers_path = join_path(think_engine_streaming_assets_path,'lib', '')

think_engine_project_path = os.path.normpath(join_path(project_path, '..', 'ThinkEngine', ''))
think_engine_src_upstream = join_path(think_engine_project_path, 'it', '')
think_engine_src = join_path(project_path, 'ThinkEngineer', 'ThinkEngine', 'ThinkEngine', 'it', '')

os_name = platform.system()
if os_name == 'Windows':
    input_path =  join_path(os.environ['USERPROFILE'], 'AppData','Local','Temp','ThinkEngineFacts', '')
else: 
    input_path = '/tmp/ThinkEngineFacts/'



