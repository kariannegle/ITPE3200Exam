import React, { useState, useEffect } from "react"
import axios from "../api/axios"
import userImg from "../assets/image.png"
import MoreImg from "../assets/more.svg"

const Comments = ({ postId }) => {
  const [comments, setComments] = useState([])
  const [newComment, setNewComment] = useState("")
  const [editingCommentId, setEditingCommentId] = useState(null)
  const [editingCommentContent, setEditingCommentContent] = useState("")

  useEffect(() => {
    const fetchComments = async () => {
      try {
        const response = await axios.get(`/comment/${postId}`)

        if (response.data && response.data.$values) {
          setComments(response.data.$values)
        } else {
          console.error("Unexpected response format:", response.data)
          setComments([])
        }
      } catch (error) {
        console.error("Error fetching comments:", error)
      }
    }

    fetchComments()
  }, [postId])

  const handleAddComment = async () => {
    if (!newComment) return

    const commentData = {
      postId,
      content: newComment,
      createdAt: new Date().toISOString(),
      username: "cool",
    }

    try {
      const response = await axios.post("/comment", commentData)
      setComments([...comments, response.data])
      setNewComment("")
    } catch (error) {
      console.error("Error adding comment:", error)
    }
  }

  const handleDeleteComment = async (commentId) => {
    try {
      await axios.delete(`/comment/${commentId}`)
      setComments(comments.filter((comment) => comment.id !== commentId))
    } catch (error) {
      console.error("Error deleting comment:", error)
    }
  }

  const handleEditComment = (commentId, content) => {
    setEditingCommentId(commentId)
    setEditingCommentContent(content)
  }

  const handleUpdateComment = async () => {
    if (!editingCommentContent) return

    try {
      const response = await axios.put(`/comment/${editingCommentId}`, {
        content: editingCommentContent,
      })
      setComments(
        comments.map((comment) =>
          comment.id === editingCommentId
            ? { ...comment, content: editingCommentContent }
            : comment
        )
      )
      setEditingCommentId(null)
      setEditingCommentContent("")
    } catch (error) {
      console.error("Error updating comment:", error)
    }
  }

  const formatDate = (dateString) => {
    return new Date(dateString).toLocaleDateString(undefined, {
      year: "numeric",
      month: "short",
      day: "numeric",
      hour: "2-digit",
      minute: "2-digit",
    })
  }

  return (
    <div>
      <h5 className="comments-header">Comments</h5>
      <div className="discussions">
        {comments.length === 0 ? (
          <p>No comments yet. Be the first to comment!</p>
        ) : (
          comments.map((comment) => (
            <div
              key={comment.id}
              className="comments-scrollable"
              aria-label="Scrollable comments list"
            >
              <div className="discussion mb-3">
                <div className="d-flex justify-content-between align-items-center">
                  <div className="d-flex align-items-center">
                    <div className="userImg d-flex align-items-center">
                      <a href="/" className="nav-link text-black">
                        <img
                          src={userImg}
                          alt="Avatar"
                          style={{ width: "25px", height: "25px" }}
                        />
                        <span className="username">{comment.username}</span>
                      </a>
                    </div>
                    <div className="comment-date">
                      <small>{formatDate(comment.createdAt)}</small>
                    </div>
                  </div>
                  <div></div>
                </div>
                <div
                  style={{ display: "flex", justifyContent: "space-between" }}
                  className="comment"
                >
                  {editingCommentId === comment.id ? (
                    <div>
                      <textarea
                        className="form-control"
                        value={editingCommentContent}
                        onChange={(e) =>
                          setEditingCommentContent(e.target.value)
                        }
                      />
                      <button
                        className="btn btn-success btn-sm mt-2"
                        onClick={handleUpdateComment}
                      >
                        Save
                      </button>
                    </div>
                  ) : (
                    <p>{comment.content}</p>
                  )}
                  <div
                    style={{ float: "right", textAlign: "center" }}
                    className="dropdown"
                  >
                    <img
                      className=" btn-secondary dropdown-toggle"
                      type="button"
                      data-bs-toggle="dropdown"
                      aria-expanded="false"
                      src={MoreImg}
                      alt="more"
                    />
                    <ul className="dropdown-menu">
                      <li>
                        <button
                          className="dropdown-item text-danger d-flex align-items-center"
                          onClick={() => handleDeleteComment(comment.id)}
                        >
                          Delete
                        </button>
                      </li>
                      <li>
                        <button
                          className="dropdown-item text-danger d-flex align-items-center"
                          onClick={() =>
                            handleEditComment(comment.id, comment.content)
                          }
                        >
                          Edit
                        </button>
                      </li>
                    </ul>
                  </div>
                </div>
              </div>
            </div>
          ))
        )}
      </div>

      <textarea
        className="form-control"
        type="text"
        value={newComment}
        onChange={(e) => setNewComment(e.target.value)}
        placeholder="Add a comment"
      />
      <button
        className="submitcomment btn btn-primary mt-3"
        onClick={handleAddComment}
      >
        Submit
      </button>
    </div>
  )
}

export default Comments
